using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Momentum.BackOffice.Account;
using Momentum.BackOffice.Data;
using Polly;
using Refit;

namespace Momentum.BackOffice.Extensions;

public static class WebAssemblyHostExtensions
{
	#region Public Methods

	internal static WebApplicationBuilder AddBlazor(this WebApplicationBuilder builder)
	{
		builder
			.Services
			.AddRazorComponents()
			.AddInteractiveServerComponents();

		builder.Services.AddBlazorBootstrap();

		return builder;
	}

	internal static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
	{
		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder
			.Services
        .AddDbContext<MomentumIdentityDbContext>((options) =>
        {
            options.UseSqlServer(connectionString, x =>
            {
                x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
            });
        });
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

		return builder;
	}

	internal static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
	{
		builder.Services.AddCascadingAuthenticationState();
		builder.Services.AddScoped<IdentityUserAccessor>();
		builder.Services.AddScoped<IdentityRedirectManager>();
		builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

		builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = IdentityConstants.ApplicationScheme;
				options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
			})
			.AddIdentityCookies();

		builder
			.Services
			.AddIdentityCore<ApplicationUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			})
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<MomentumIdentityDbContext>()
			.AddSignInManager()
			.AddDefaultTokenProviders();

		builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

		return builder;
	}

	#endregion // Public Methods

	internal static WebApplicationBuilder AddClientServices(this WebApplicationBuilder builder)
	{
		builder
			.Services
			.AddOptions()
			.AddLocalization()
			.AddValidatorsFromAssembly(typeof(IAssemblyMarker).Assembly);

		var serverUri = new Uri(builder.Configuration["ApiServerUrl"]!);
		AddRefitClients<IAssemblyMarker>(builder.Services, serverUri);

		return builder;
	}

	#region Private Methods

	private static void AddRefitClients<TAssemblyMarker>(IServiceCollection services,
		Uri serverUri)
	{
		var types = typeof(TAssemblyMarker)
			.Assembly
			.GetExportedTypes()
			.Where(t => t.IsInterface && t.Name.EndsWith("Service"))
			.Select(t => new
			{
				RefitService = t
			});

		var settings = new RefitSettings
		{
			ContentSerializer = new NewtonsoftJsonContentSerializer(
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				})
		};
		foreach (var type in types)
		{
			

			services
				.AddRefitClient(type.RefitService, settings)
				.ConfigureHttpClient(c =>
				{
					c.BaseAddress = serverUri;
					c.Timeout = TimeSpan.FromSeconds(5);
				})
				.AddTransientHttpErrorPolicy(b => b.WaitAndRetryAsync(
				[
					TimeSpan.FromSeconds(1),
					TimeSpan.FromSeconds(2)
				]));
		}
	}

	#endregion // Private Methods
}