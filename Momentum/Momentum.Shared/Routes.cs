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

	public static class Products
	{
		public const string Get = "/product";
		public const string Insert = "/product";
		public const string Update = "/product/{id}";
		public const string Delete = "/product/{id}";
	}

	public static class Events
	{
		public const string Get = "/event";
		public const string Insert = "/event";
		public const string Update = "/event/{id}";
		public const string Delete = "/event/{id}";
	}

	public static class EventOrders
	{
		public const string Get = "/eventorder";
		public const string GetById = "/eventorder/{id}";
		public const string Create = "/eventorder";
		public const string Update = "/eventorder/{id}";
		public const string Delete = "/eventorder/{id}";
	}
}