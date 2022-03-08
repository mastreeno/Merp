using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Acl.RegistryResolutionServices;
using MementoFX.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddLocalization();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<Resolver>();

builder.Services.AddSingleton(builder.Services);

ConfigureRegistryBoundedContext(builder);

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