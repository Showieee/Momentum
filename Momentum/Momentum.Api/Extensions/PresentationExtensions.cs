using Momentum.Persistence.Extensions;
using Momentum.Services.Extensions;

namespace Momentum.Api.Extensions;

public static class PresentationExtensions
{
	public static IServiceCollection AddPresentation(this IServiceCollection services,
		IConfiguration configuration)
	{

		services
			.AddPersistence(configuration)
			.AddServices(configuration);

		return services;
	}
}