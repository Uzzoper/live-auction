using LiveAuction.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LiveAuction.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestDatabase = "LiveAuction_Tests";

    public CustomWebApplicationFactory()
    {
        // Environment variables override appsettings, so the API uses the test database
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection",
            $"Server=localhost,1433;Database={TestDatabase};User Id=sa;Password=Dev_Password123;TrustServerCertificate=True");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Safety guard: never delete anything but the test database
        if (db.Database.GetDbConnection().Database != TestDatabase)
            throw new InvalidOperationException("Integration tests must run against the test database.");

        db.Database.EnsureDeleted();
        db.Database.Migrate();

        return host;
    }
}
