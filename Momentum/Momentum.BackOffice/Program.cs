using Momentum.BackOffice;
using Momentum.BackOffice.Account;
using Momentum.BackOffice.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
	.AddBlazor()
	.AddPersistence()
	.AddAuthentication()
	.AddClientServices();

var app = builder.Build();

await app.DoMigrationAndSeeding();

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

await app.RunAsync();