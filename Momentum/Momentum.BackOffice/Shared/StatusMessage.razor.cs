using Microsoft.AspNetCore.Components;
using Momentum.BackOffice.Account;

namespace Momentum.BackOffice.Shared;

public partial class StatusMessage
{
	#region Fields

	private string? _messageFromCookie;

	#endregion //Fields

	#region Parameters

	[Parameter] public string? Message { get; set; }

	#endregion //Parameters

	#region Private Properties

	[CascadingParameter] private HttpContext HttpContext { get; set; } = null!;

	private string? DisplayMessage => Message ?? _messageFromCookie;

	#endregion //Private Properties

	#region Private Methods

	protected override void OnInitialized()
	{
		_messageFromCookie = HttpContext.Request.Cookies[IdentityRedirectManager.StatusCookieName];

		if (_messageFromCookie is not null)
		{
			HttpContext.Response.Cookies.Delete(IdentityRedirectManager.StatusCookieName);
		}
	}

	#endregion //Private Methods
}