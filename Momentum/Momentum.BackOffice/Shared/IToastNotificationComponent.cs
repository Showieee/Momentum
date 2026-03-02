using BlazorBootstrap;

namespace Momentum.BackOffice.Shared;

public interface IToastNotificationComponent
{
	Task ShowMessage(ToastType toastType, string title, string message);
}