using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using FluentValidation;
using ClinicFlow.Application.Greetings;
using ClinicFlow.Application.Users;

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

app.MapPost("/token", (UserCredential credential) =>
{
    if (credential.Username == "admin" && credential.Password == "password")
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, credential.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(new { token = tokenValue });
    }

    return Results.Unauthorized();
});

app.Run();

record UserCredential(string Username, string Password);

public partial class Program { }
