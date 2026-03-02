using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Momentum.BackOffice.Data;

namespace Momentum.BackOffice.Account;
internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost($"/{PageRoutes.Account.Logout}", async (
			[FromServices] SignInManager<ApplicationUser> signInManager,
			[FromForm] string returnUrl) =>
		{
			await signInManager.SignOutAsync();
			return TypedResults.LocalRedirect($"~/{returnUrl}");
		});

		return endpoints;
	}
}