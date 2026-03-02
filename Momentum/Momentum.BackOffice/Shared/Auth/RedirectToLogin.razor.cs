using Microsoft.AspNetCore.Components;

namespace Momentum.BackOffice.Shared.Auth;

public partial class RedirectToLogin
{
	#region Private Properties

	[Inject] private NavigationManager NavigationManager { get; set; } = null!;

	#endregion //Private Properties

	#region Private Methods

	protected override void OnInitialized()
	{
		NavigationManager.NavigateTo($"{PageRoutes.Account.Login}?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}", forceLoad: true);
	}

	#endregion //Private Methods
}