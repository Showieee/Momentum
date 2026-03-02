using Microsoft.JSInterop;

namespace Momentum.BackOffice.Extensions;

public static class JsInterop
{
	internal static ValueTask DownloadFileFromStream(this IJSRuntime jsRuntime, string fileName, Stream stream)
	{
		var dotnetStream = new DotNetStreamReference(stream);
		return jsRuntime.InvokeVoidAsync("common.downloadFileFromStreamAsync", fileName, dotnetStream);
	}
}