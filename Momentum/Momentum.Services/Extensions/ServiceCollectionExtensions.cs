using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Momentum.Services.Interfaces;

namespace Momentum.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICompanyService, CompanyService>();

        return services;
    }
}