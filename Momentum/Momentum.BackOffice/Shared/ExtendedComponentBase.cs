using System.Security.Claims;
using System.Text.Json;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;

namespace Momentum.BackOffice.Shared;

public abstract class ExtendedComponentBase : ComponentBase
{
	#region Private Properties

	[CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = null!;

	[CascadingParameter] protected IToastNotificationComponent ToastNotificationComponent { get; set; } = null!;

	[Inject] protected PreloadService PreloadService { get; set; } = null!;

	[Inject] protected NavigationManager NavigationManager { get; set; } = null!;
	
	protected ClaimsPrincipal? UserPrincipal { get; set; }

	protected bool UserIsAuthenticated { get; set; }

	#endregion //Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		var authState = await AuthState;
		if (authState.User is { Identity: ClaimsIdentity { IsAuthenticated: true } })
		{
			UserIsAuthenticated = true;
			UserPrincipal = authState.User;
		}
	}

	protected async Task SafeExecute(Func<Task> func, Action? onTokenError = null, Func<Exception, Task<bool>>? onError = null)
	{
		try
		{
			await func.Invoke();
		}
		catch (Exception exc)
		{
			var handled = false;
			if (onError != null)
			{
				handled = await onError.Invoke(exc);
			}

			if (!handled)
			{
				await ShowException(exc);
			}
		}
	}

	protected Task ShowSuccess(string message, string? title = null)
	{
		var successTitle = title ?? "Success";
		var successMessage = message ?? string.Empty;

		return ToastNotificationComponent.ShowMessage(ToastType.Success, successTitle, successMessage);
	}
	
	protected Task ShowWarning(string message, string? title = null)
	{
		var successTitle = title ?? "Success";
		var successMessage = message ?? string.Empty;

		return ToastNotificationComponent.ShowMessage(ToastType.Warning, successTitle, successMessage);
	}

	protected Task ShowException(Exception exc, string? title = null)
	{
		var errorTitle = title ?? "Error";
		var errorMessage = exc.InnerException?.Message ?? exc.Message;

		return ToastNotificationComponent.ShowMessage(ToastType.Danger, errorTitle, errorMessage);
	}

	protected Task ShowException(ApiException exc, string? title = null)
	{
		var errorTitle = title ?? "Error";

		var exceptionMessage = exc.InnerException?.Message ?? exc.Message;
		var errorMessage = exceptionMessage;
		try
		{
			if (exc.Content is not null)
			{
				var problem = JsonSerializer.Deserialize<ProblemDetails>(exc.Content);
				if (problem?.Extensions is not null && problem.Extensions.TryGetValue("errors", out var errorsElement) && errorsElement is JsonElement errorsJsonElement)
				{
					if (errorsJsonElement.ValueKind == JsonValueKind.Array && errorsJsonElement.GetArrayLength() > 0)
					{
						var firstError = errorsJsonElement[0];
						if (firstError.TryGetProperty("message", out var messageElement))
						{
							errorMessage = messageElement.GetString() ?? errorMessage;
						}

						if (firstError.TryGetProperty("code", out var codeElement))
						{
							errorMessage = $"({codeElement}) {errorMessage}";
						}
					}
				}
			}
		}
		catch (JsonException)
		{
		}

		return ToastNotificationComponent.ShowMessage(ToastType.Danger, errorTitle, errorMessage);
	}

	#endregion //Private Methods
}