namespace Momentum.Shared;

public static class Routes
{
	public static class Persons
	{
		public const string Get = "/person";
		public const string Insert = "/person";
		public const string Update = "/person/{id}";
		public const string Delete = "/person/{id}";
	}

	public static class Companies
	{
		public const string Get = "/company";
		public const string Insert = "/company";
		public const string Update = "/company/{id}";
		public const string Delete = "/company/{id}";
	}
}