using Momentum.Persistence.Extensions;

namespace Momentum.Api.Extensions;

public static class PresentationExtensions
{
	public static IServiceCollection AddPresentation(this IServiceCollection services,
		IConfiguration configuration)
	{

		services
			.AddPersistence(configuration);

		return services;
	}
}