using System.Net.Http.Json;

namespace LiveAuction.IntegrationTests;

internal static class AuthHelper
{
    private record LoginBody(Guid UserId, string DisplayName);

    public static async Task<(HttpClient Client, Guid UserId)> CreateAuthenticatedClientAsync(
        CustomWebApplicationFactory factory)
    {
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid():N}@test.com";
        const string password = "Senha12345";

        var register = await client.PostAsJsonAsync("/auth/register",
            new { email, displayName = "Test user", password });
        register.EnsureSuccessStatusCode();

        var login = await client.PostAsJsonAsync("/auth/login", new { email, password });
        login.EnsureSuccessStatusCode();

        var body = await login.Content.ReadFromJsonAsync<LoginBody>();
        return (client, body!.UserId);
    }
}
