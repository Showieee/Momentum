namespace Momentum.BackOffice;

internal class PageRoutes
{
	public const string Dashboard = "";
	public const string UserClaims = "user-claims";
	public const string Persons = "persons";

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