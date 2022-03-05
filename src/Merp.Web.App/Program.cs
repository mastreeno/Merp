using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Acl.RegistryResolutionServices;
using MementoFX.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<Resolver>();



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
    //Services
    builder.Services.AddSingleton<Merp.Registry.Web.App.Services.UrlBuilder>();

    //Read Model
    var readModelConnectionString = builder.Configuration.GetValue<string>("Modules:Registry:ConnectionStrings:ReadModel");
    builder.Services.AddDbContext<Merp.Registry.QueryStack.RegistryDbContext>(options => options.UseSqlServer(readModelConnectionString));
    builder.Services.AddScoped<Merp.Registry.QueryStack.IDatabase, Merp.Registry.QueryStack.Database>();

    //Event Store
    //var mongoDbConnectionString = builder.Configuration.GetValue<string>("Modules:Registry:ConnectionStrings:EventStore");
    //var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
    //var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
    //builder.Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
    //builder.Services.AddScoped<IEventStore, MementoFX.Persistence.MongoDB.MongoDbEventStore>();
    //builder.Services.AddScoped<IRepository, MementoFX.Persistence.Repository>();
}