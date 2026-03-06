using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Momentum.BackOffice.Shared;

namespace Momentum.BackOffice.Layout;

public partial class MainLayout : IToastNotificationComponent
{
	#region Fields

	private IList<NavItem> _navItems = [];
	private readonly List<ToastMessage> _messages = [];
	private Modal _modal = null!;

	#endregion //Fields

	#region Private Properties

	[CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = null!;

	#endregion //Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
	}

	protected async Task<SidebarDataProviderResult> SidebarDataProvider(SidebarDataProviderRequest request)
	{
		if (_navItems.Count == 0)
		{
			_navItems = await GetNavItems();
		}

		return request.ApplyTo(_navItems);
	}

	private async Task<IList<NavItem>> GetNavItems()
	{
		_navItems =
		[
			new() { Id = "1", Href = "/", IconName = IconName.HouseDoorFill, Text = "Dashboard", Match=NavLinkMatch.All},
		];

		await AddAuthenticatedNavItems();

		return _navItems;
	}

	private async Task AddAuthenticatedNavItems()
	{
		var authState = await AuthState;
		var user = authState.User;
		var isAuthenticated = user.Identity?.IsAuthenticated == true;

		if (isAuthenticated && user.IsInRole(Roles.Administrator))
		{
			_navItems.Add(new NavItem { Id = "10", Href = PageRoutes.Persons, IconName = IconName.Gear, Text = "Persons" });
		}
	}

	#endregion //Private Methods

	#region IToastNotificationComponent Members

	public async Task ShowMessage(ToastType toastType, string title, string message)
	{
		var toastMessage = new ToastMessage
		{
			Type = toastType,
			//Title = title,
			Message = message
		};

		_messages.Add(toastMessage);
		await InvokeAsync(StateHasChanged);
	}

	#endregion //IToastNotificationComponent Members

	#region IModalService

	public async Task ShowModal<T>(string title, Dictionary<string, object> parameters)
	{
		await _modal.ShowAsync<T>(title: title, parameters: parameters);
	}

	#endregion //IModalService
}