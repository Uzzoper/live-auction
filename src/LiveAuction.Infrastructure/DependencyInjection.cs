using LiveAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LiveAuction.Domain.Repositories;
using LiveAuction.Infrastructure.Repositories;

namespace LiveAuction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
