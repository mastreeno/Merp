using Acl.RegistryResolutionServices;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddLocalization();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<Resolver>();

builder.Services
    .AddScoped<Merp.Registry.Web.Api.Internal.IPartyApiServices, Merp.Registry.Web.Api.Internal.PartyApiServices>()
    .AddScoped<Merp.Registry.Web.Api.Internal.IPersonApiServices, Merp.Registry.Web.Api.Internal.PersonApiServices>();

builder.Services.AddScoped<Merp.Web.IAppContext, Merp.Web.App.MockAppContext>();

builder.Services.AddSingleton(builder.Services);

ConfigureRegistryBoundedContext(builder);
ConfigureAccountancyBoundedContext(builder);

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

var supportedLanguages = new[] { "en", "it" };
app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures(supportedLanguages)
    .AddSupportedUICultures(supportedLanguages));

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

static void ConfigureRegistryBoundedContext(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<Merp.Registry.Web.Core.Configuration.IBoundedContextConfigurationProvider, Merp.Registry.Web.App.Configuration.BoundedContextConfigurationProvider>();
    builder.Services.AddScoped<Merp.Registry.Web.AppBootstrapper>();

    var bootstrapper = builder.Services.BuildServiceProvider().GetService<Merp.Registry.Web.AppBootstrapper>();
    bootstrapper.Configure();

    builder.Services.AddScoped<Merp.Registry.Web.App.Services.UrlBuilder>();
}

static void ConfigureAccountancyBoundedContext(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<Merp.Accountancy.Web.Core.Configuration.IBoundedContextConfigurationProvider, Merp.Accountancy.Web.App.Configuration.BoundedContextConfigurationProvider>();
    builder.Services.AddScoped<Merp.Accountancy.Web.AppBootstrapper>();

    var bootstrapper = builder.Services.BuildServiceProvider().GetService<Merp.Accountancy.Web.AppBootstrapper>();
    bootstrapper.Configure();

    builder.Services.AddScoped<Merp.Accountancy.Web.App.Services.UrlBuilder>();
}