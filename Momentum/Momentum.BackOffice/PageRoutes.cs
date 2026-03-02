namespace Momentum.BackOffice;

internal class PageRoutes
{
	public const string Dashboard = "";
	public const string UserClaims = "user-claims";
	public const string Options = "options";
	public const string Payments = "payments";
	public const string CustomerAudits = "audits";
	public const string Whitelist = "whitelist";
	public const string AnafReports = "anaf-reports";

	internal class Authentication
	{
		public const string Name = "authentication";

		public const string Login = $"{Name}/Login";
		public const string Logout = $"{Name}/Logout";
	}

	internal class Account
	{
		public const string Name = "account";

		public const string Login = $"{Name}/login";
		public const string Lockout = $"{Name}/lockout";
		public const string Logout = $"{Name}/logout";
		public const string InvalidUser = $"{Name}/invalidUser";
	}

	internal class BackOffice
	{
		public const string Name = "backoffice";
	}
}