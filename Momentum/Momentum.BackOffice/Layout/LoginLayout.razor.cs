using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Momentum.BackOffice.Layout;

public partial class LoginLayout
{
	#region Private Properties

	[CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = null!;

	#endregion //Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
	}

	#endregion //Private Methods
}