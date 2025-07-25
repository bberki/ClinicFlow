using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using ClinicFlow.Application.Greetings;
using ClinicFlow.Application.Auth;
using ClinicFlow.Infrastructure.Repositories;
using ClinicFlow.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

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

app.MapPost("/token", async (UserCredential credential, IUserRepository users, ITokenService tokenService) =>
{
    var user = await users.GetByCredentialsAsync(credential.Username, credential.Password);
    if (user is not null)
    {
        var tokenValue = tokenService.GenerateToken(user);
        return Results.Ok(new { token = tokenValue });
    }

    return Results.Unauthorized();
});

app.Run();

record UserCredential(string Username, string Password);

public partial class Program { }
