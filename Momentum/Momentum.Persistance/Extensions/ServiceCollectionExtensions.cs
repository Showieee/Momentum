using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Momentum.Persistance;

namespace Momentum.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, 
	    IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MomentumDbContext>((options) =>
        {
			options.UseSqlServer(connectionString, x =>
            {
                x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
            });
        });

        return services;
    }
}