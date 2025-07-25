using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using Serilog;
using ClinicFlow.Application.Greetings;
using ClinicFlow.Application.Users;
using ClinicFlow.Application.Common;
using ClinicFlow.Infrastructure.Data;
using ClinicFlow.Infrastructure.Repositories;
using ClinicFlow.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

builder.Services.Configure<JwtSettings>(jwtSection);
builder.Services.AddDbContext<ClinicFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetGreetingQuery>());
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommand>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/public", () => "Public endpoint");

app.MapGet("/greeting", async (IMediator mediator) =>
{
    var message = await mediator.Send(new GetGreetingQuery());
    return Results.Ok(message);
});

app.MapGet("/protected", [Authorize]() => "You are authenticated!");

app.MapGet("/admin", [Authorize(Roles = "Admin")] (ClaimsPrincipal user) =>
    $"Hello {user.Identity?.Name}, you are an admin.");

var authGroup = app.MapGroup("/auth");

authGroup.MapPost("/register", async (IMediator mediator, RegisterUserCommand command) =>
{
    await mediator.Send(command);
    return Results.Ok();
});

authGroup.MapPost("/login", async (IMediator mediator, LoginQuery query) =>
{
    var token = await mediator.Send(query);
    if (string.IsNullOrEmpty(token))
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new { token });
});


app.Run();

public partial class Program { }
