using Microsoft.EntityFrameworkCore;
using Momentum.BackOffice.Data;

namespace Momentum.BackOffice.Extensions;

public static class WebApplicationExtensions
{
	internal static async Task DoMigrationAndSeeding(this WebApplication app)
	{
		await using var scope = app.Services.CreateAsyncScope();
		var services = scope.ServiceProvider;
		var context = services.GetRequiredService<MomentumIdentityDbContext>();
		await context.Database.MigrateAsync();
		await DbSeeding.Run(services);
	}
}