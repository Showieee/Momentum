using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Momentum.BackOffice.Account;
using Momentum.BackOffice.Data;

namespace Momentum.BackOffice.Pages.Account;

[Route($"/{PageRoutes.Account.Login}")]
public partial class Login
{
	#region Fields

	private string? _errorMessage;

	#endregion //Fields

	#region Public Methods

	public async Task LoginUser()
	{
		var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
		if (result.Succeeded)
		{
			Logger.LogInformation("User logged in.");
			RedirectManager.RedirectTo(ReturnUrl);
		}
		else if (result.IsLockedOut)
		{
			Logger.LogWarning("User account locked out.");
			RedirectManager.RedirectTo(PageRoutes.Account.Lockout);
		}
		else
		{
			_errorMessage = "Error: Invalid login attempt.";
		}
	}

	#endregion //Public Methods

	#region Private Properties

	[CascadingParameter] private HttpContext HttpContext { get; set; } = null!;
	[SupplyParameterFromForm] private InputModel Input { get; set; } = new();
	[SupplyParameterFromQuery] private string? ReturnUrl { get; set; }
	[Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = null!;
	[Inject] private ILogger<Login> Logger { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] private IdentityRedirectManager RedirectManager { get; set; } = null!;

	#endregion //Private Properties

	protected override async Task OnInitializedAsync()
	{
		if (HttpMethods.IsGet(HttpContext.Request.Method))
		{
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
		}
	}

	private sealed class InputModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = "";

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = "";

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}