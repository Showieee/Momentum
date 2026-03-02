using Microsoft.AspNetCore.Components;

namespace Momentum.BackOffice.Shared.Auth;

public partial class LogInOrOut
{
	private string? _currentUrl;

	[Inject] private NavigationManager Navigation { get; set; } = null!;

	protected override void OnInitialized()
	{
		_currentUrl = Navigation.BaseUri;
	}
}