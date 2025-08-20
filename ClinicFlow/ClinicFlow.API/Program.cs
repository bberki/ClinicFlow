using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MediatR;
using FluentValidation;
using Serilog;
using ClinicFlow.Application.Greetings;
using ClinicFlow.Application.Users;
using ClinicFlow.Application.Common;
using ClinicFlow.Application.Appointments;
using System.Security.Claims;
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
builder.Services.AddMediatR(typeof(GetGreetingQuery).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommand>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClinicFlow API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!")
    .WithName("GetRoot")
    .WithTags("General")
    .Produces<string>(StatusCodes.Status200OK);

app.MapGet("/public", () => "Public endpoint")
    .WithName("GetPublic")
    .WithTags("General")
    .Produces<string>(StatusCodes.Status200OK);

app.MapGet("/greeting", async (IMediator mediator) =>
{
    var message = await mediator.Send(new GetGreetingQuery());
    return Results.Ok(message);
})
    .WithName("GetGreeting")
    .WithTags("Greetings")
    .Produces<string>(StatusCodes.Status200OK);

app.MapGet("/protected", [Authorize]() => "You are authenticated!")
    .WithName("GetProtected")
    .WithTags("Auth")
    .Produces<string>(StatusCodes.Status200OK);

app.MapGet("/admin", [Authorize(Roles = "Admin")] (ClaimsPrincipal user) =>
    $"Hello {user.Identity?.Name}, you are an admin.")
    .WithName("GetAdmin")
    .WithTags("Auth")
    .Produces<string>(StatusCodes.Status200OK);

var authGroup = app.MapGroup("/auth");

authGroup.MapPost("/register", async (IMediator mediator, RegisterUserCommand command) =>
{
    await mediator.Send(command);
    return Results.Ok();
})
    .WithName("RegisterUser")
    .WithTags("Authentication")
    .Produces(StatusCodes.Status200OK);

authGroup.MapPost("/login", async (IMediator mediator, LoginQuery query) =>
{
    var token = await mediator.Send(query);
    if (string.IsNullOrEmpty(token))
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new { token });
})
    .WithName("Login")
    .WithTags("Authentication")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

app.MapPost("/appointments", [Authorize] async (
    IMediator mediator,
    ClaimsPrincipal user,
    CreateAppointmentCommand command) =>
{
    var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userIdClaim))
    {
        return Results.Unauthorized();
    }

    var userId = int.Parse(userIdClaim);
    var result = await mediator.Send(command with { UserId = userId });
    return Results.Ok(new { id = result });
})
    .WithName("CreateAppointment")
    .WithTags("Appointments")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);


app.Run();

public partial class Program { }
