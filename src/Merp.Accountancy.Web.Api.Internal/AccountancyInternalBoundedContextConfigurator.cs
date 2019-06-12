using MementoFX.Persistence;
using Merp.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Merp.Accountancy.Web.Api.Internal.WorkerServices;

namespace Merp.Accountancy.Web.Api.Internal
{
    public class AccountancyInternalBoundedContextConfigurator : BoundedContextConfigurator
    {
        public AccountancyInternalBoundedContextConfigurator(IConfiguration configuration, IServiceCollection services)
            : base(configuration, services)
        {

        }

        protected override void ConfigureEventStore()
        {
            var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Accountancy-EventStore");
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
            Services.AddTransient<IEventStore, MementoFX.Persistence.MongoDB.MongoDbEventStore>();
            Services.AddTransient<IRepository, MementoFX.Persistence.Repository>();
        }

        protected override void RegisterAclServices()
        {
            //Acl Services
        }

        protected override void RegisterDenormalizers()
        {
            //Denormalizers
        }

        protected override void RegisterHandlers()
        {
            //Handlers
        }

        protected override void RegisterSagas()
        {
            //Sagas
        }

        protected override void RegisterServices()
        {
            //Services
        }

        protected override void RegisterTypes()
        {
            //Types
            var readModelConnectionString = Configuration.GetConnectionString("Merp-Accountancy-ReadModel");
            Services.AddDbContext<Merp.Accountancy.QueryStack.AccountancyDbContext>(options => options.UseSqlServer(readModelConnectionString));
            Services.AddScoped<Merp.Accountancy.QueryStack.IDatabase, Merp.Accountancy.QueryStack.Database>();

            var settingsConnectionString = Configuration.GetConnectionString("Merp-Accountancy-Settings");
            Services.AddDbContext<Merp.Accountancy.Settings.AccountancySettingsDbContext>(options => options.UseSqlServer(settingsConnectionString));
            Services.AddScoped<Merp.Accountancy.Settings.IDatabase, Merp.Accountancy.Settings.Database>();
            Services.AddScoped<Merp.Accountancy.Settings.Commands.VatCommands>();
            Services.AddScoped<Merp.Accountancy.Settings.Commands.SettingsDefaultsCommands>();
        }

        protected override void RegisterWorkerServices()
        {
            //Worker services
            Services.AddScoped<VatControllerWorkerServices>();
            Services.AddScoped<ProvidenceFundControllerWorkerServices>();
            Services.AddScoped<WithholdingTaxControllerWorkerServices>();
            Services.AddScoped<SettingsControllerWorkerServices>();
        }

        protected override void SubscribeEvents()
        {
            //Events
        }
    }
}
