using System.Net.Http.Json;
using ClinicFlow.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ClinicFlow.Tests.Api;

public class AuthEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterAndLogin_Workflow()
    {
        var registerResponse = await _client.PostAsJsonAsync("/auth/register", new { username = "apiuser", password = "secret" });
        registerResponse.EnsureSuccessStatusCode();

        var loginResponse = await _client.PostAsJsonAsync("/auth/login", new { username = "apiuser", password = "secret" });
        loginResponse.EnsureSuccessStatusCode();
        var json = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.False(string.IsNullOrEmpty(json?.token));
    }

    private record TokenResponse(string token);
}

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ClinicFlowDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ClinicFlowDbContext>(options => options.UseInMemoryDatabase("api_test"));
        });
    }
}
