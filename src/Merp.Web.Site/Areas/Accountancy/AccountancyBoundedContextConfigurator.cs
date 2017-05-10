using Memento.Persistence;
using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Accountancy.CommandStack.Services;
using Merp.Accountancy.QueryStack.Denormalizers;
using Merp.Web.Site.Areas.Accountancy.WorkerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Rebus.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Merp.Web.Site.Areas.Accountancy
{
    public class AccountancyBoundedContextConfigurator : AppBootstrapper.BoundedContextConfigurator
    {
        public AccountancyBoundedContextConfigurator(IConfigurationRoot configuration, IServiceCollection services) : base(configuration, services)
        {

        }

        protected override void ConfigureEventStore()
        {
            var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Accountancy-EventStore");
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
            Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
            Services.AddTransient<IRepository, Memento.Persistence.Repository>();
        }

        protected override void SubscribeEvents()
        {
            //Events
            Bus.Subscribe<FixedPriceJobOrderCompletedEvent>();
            Bus.Subscribe<FixedPriceJobOrderExtendedEvent>();
            Bus.Subscribe<FixedPriceJobOrderRegisteredEvent>();
            Bus.Subscribe<IncomingInvoiceLinkedToJobOrderEvent>();
            Bus.Subscribe<IncomingInvoiceRegisteredEvent>();
            Bus.Subscribe<IncomingInvoicePaidEvent>();
            Bus.Subscribe<IncomingInvoiceExpiredEvent>();
            Bus.Subscribe<OutgoingInvoiceIssuedEvent>();
            Bus.Subscribe<OutgoingInvoiceLinkedToJobOrderEvent>();
            Bus.Subscribe<OutgoingInvoicePaidEvent>();
            Bus.Subscribe<OutgoingInvoiceExpiredEvent>();
            Bus.Subscribe<TimeAndMaterialJobOrderCompletedEvent>();
            Bus.Subscribe<TimeAndMaterialJobOrderExtendedEvent>();
            Bus.Subscribe<TimeAndMaterialJobOrderRegisteredEvent>();
        }

        protected override void RegisterDenormalizers()
        {
            //Denormalizers
            Services.AutoRegisterHandlersFromAssemblyOf<FixedPriceJobOrderDenormalizer>();
        }

        protected override void RegisterHandlers()
        {
            //Handlers
        }

        protected override void RegisterSagas()
        {
            //Sagas
            Services.AutoRegisterHandlersFromAssemblyOf<FixedPriceJobOrderSaga>();
        }

        protected override void RegisterServices()
        {
            //Services
            Services.AddScoped<IJobOrderNumberGenerator, JobOrderNumberGenerator>();
            Services.AddScoped<IOutgoingInvoiceNumberGenerator, OutgoingInvoiceNumberGenerator>();
        }

        protected override void RegisterTypes()
        {
            //Types
            var readModelConnectionString = Configuration.GetConnectionString("Merp-Accountancy-ReadModel");

            Services.AddScoped<Merp.Accountancy.QueryStack.AccountancyContext>((s) => new Merp.Accountancy.QueryStack.AccountancyContext(readModelConnectionString));
            Services.AddScoped<Merp.Accountancy.QueryStack.IDatabase, Merp.Accountancy.QueryStack.Database>((s) => new Merp.Accountancy.QueryStack.Database(readModelConnectionString));
        }

        protected override void RegisterWorkerServices()
        {
            //Worker Services
            Services.AddScoped<InvoiceControllerWorkerServices, InvoiceControllerWorkerServices>();
            Services.AddScoped<JobOrderControllerWorkerServices, JobOrderControllerWorkerServices>();
        }
    }
}
