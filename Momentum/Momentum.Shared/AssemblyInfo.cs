using System.Reflection;

namespace Momentum.Shared;

public static class AssemblyInfo
{
	public static string Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";
}