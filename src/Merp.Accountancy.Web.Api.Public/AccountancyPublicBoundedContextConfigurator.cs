using MementoFX.Persistence;
using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Accountancy.CommandStack.Services;
using Merp.Accountancy.QueryStack.Denormalizers;
using Merp.Accountancy.Web.Api.Public.WorkerServices;
using Merp.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public
{
    public class AccountancyPublicBoundedContextConfigurator : BoundedContextConfigurator
    {
        public AccountancyPublicBoundedContextConfigurator(IConfiguration configuration, IServiceCollection services)
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
            //Services.AddTransient<IEventStore, MementoFX.Persistence.MongoDB.MongoDbSingleCollectionEventStore>();
            Services.AddTransient<IRepository, MementoFX.Persistence.Repository>();
        }

        protected override void RegisterAclServices()
        {
            //Acl Services
        }

        protected override void RegisterDenormalizers()
        {
            //Denormalizers
            Services.AutoRegisterHandlersFromAssemblyOf<JobOrderDenormalizer>();
        }

        protected override void RegisterHandlers()
        {
            //Handlers
        }

        protected override void RegisterSagas()
        {
            //Sagas
            Services.AutoRegisterHandlersFromAssemblyOf<JobOrderSaga>();
        }

        protected override void RegisterServices()
        {
            //Services
            Services.AddScoped<IJobOrderNumberGenerator, JobOrderNumberGenerator>();
            Services.AddScoped<IOutgoingInvoiceNumberGenerator, OutgoingInvoiceNumberGenerator>();
            Services.AddScoped<IOutgoingCreditNoteNumberGenerator, OutgoingCreditNoteNumberGenerator>();
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
            Services.AddScoped<InvoiceControllerWorkerServices>();
            Services.AddScoped<JobOrderControllerWorkerServices>();
        }

        protected override void SubscribeEvents()
        {
            //Events
            Bus.Subscribe<JobOrderCompletedEvent>();
            Bus.Subscribe<JobOrderExtendedEvent>();
            Bus.Subscribe<JobOrderRegisteredEvent>();
            Bus.Subscribe<IncomingInvoiceLinkedToJobOrderEvent>();
            Bus.Subscribe<IncomingCreditNoteRegisteredEvent>();
            Bus.Subscribe<IncomingCreditNoteLinkedToJobOrderEvent>();
            Bus.Subscribe<IncomingInvoiceRegisteredEvent>();
            Bus.Subscribe<IncomingInvoicePaidEvent>();
            Bus.Subscribe<IncomingInvoiceOverdueEvent>();
            Bus.Subscribe<OutgoingInvoiceIssuedEvent>();
            Bus.Subscribe<OutgoingInvoiceLinkedToJobOrderEvent>();
            Bus.Subscribe<OutgoingInvoicePaidEvent>();
            Bus.Subscribe<OutgoingInvoiceOverdueEvent>();
            Bus.Subscribe<OutgoingCreditNoteIssuedEvent>();
            Bus.Subscribe<OutgoingCreditNoteLinkedToJobOrderEvent>();
        }
    }
}
