using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ClinicFlow.Tests.Api;

public class AppointmentEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AppointmentEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAppointment_Unauthorized_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/appointments", new
        {
            patientId = 1,
            doctorId = 1,
            scheduledAt = DateTime.UtcNow.AddHours(1)
        });

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateAppointment_Authorized_ReturnsOk()
    {
        await _client.PostAsJsonAsync("/auth/register", new { username = "apptuser", password = "secret" });
        var login = await _client.PostAsJsonAsync("/auth/login", new { username = "apptuser", password = "secret" });
        login.EnsureSuccessStatusCode();
        var token = (await login.Content.ReadFromJsonAsync<TokenResponse>())!.token;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/appointments", new
        {
            patientId = 1,
            doctorId = 1,
            scheduledAt = DateTime.UtcNow.AddHours(1)
        });

        response.EnsureSuccessStatusCode();
    }

    private record TokenResponse(string token);
}
