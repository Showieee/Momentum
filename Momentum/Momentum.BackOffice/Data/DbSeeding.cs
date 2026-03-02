using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Momentum.BackOffice.Data;

public class DbSeeding
{
	private static readonly IEnumerable<SeedUser> _seedUsers =
	[
		new()
		{
			Email = "admin@momentum.com",
			NormalizedEmail = "ADMIN@MOMENTUM.COM",
			NormalizedUserName = "ADMIN@MOMENTUM.COM",
			RoleList = ["Administrator", "Manager", "User" ],
			
			UserName = "admin@momentum.com",
			Password = "Parola1234!"
		},
	];

	public static async Task Run(IServiceProvider serviceProvider)
	{
		await using var context = new MomentumIdentityDbContext(serviceProvider.GetRequiredService<DbContextOptions<MomentumIdentityDbContext>>());

		if (context.Users.Any())
		{
			return;
		}

		var userStore = new UserStore<ApplicationUser>(context);
		var password = new PasswordHasher<ApplicationUser>();

		using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		string[] roles = [Roles.Administrator, Roles.Manager, Roles.User];

		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
			}
		}

		using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

		foreach (var user in _seedUsers)
		{
			user.PasswordHash = password.HashPassword(user, user.Password);
			await userStore.CreateAsync(user);

			if (user.Email is not null)
			{
				var appUser = await userManager.FindByEmailAsync(user.Email);

				if (appUser is not null && user.RoleList is not null)
				{
					await userManager.AddToRolesAsync(appUser, user.RoleList);
				}
			}
		}

		await context.SaveChangesAsync();
	}

	private class SeedUser : ApplicationUser
	{
		public required string[]? RoleList { get; init; }
		public required string Password { get; init; }
	}
}