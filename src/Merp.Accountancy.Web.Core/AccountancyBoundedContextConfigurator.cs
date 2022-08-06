using MementoFX.Persistence;
using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Accountancy.CommandStack.Services;
using Merp.Accountancy.QueryStack.Denormalizers;
using Merp.Accountancy.Web.Core.Configuration;
using Merp.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;

namespace Merp.Accountancy.Web
{
    public class AccountancyBoundedContextConfigurator : BoundedContextConfigurator
    {
        public readonly IBoundedContextConfigurationProvider BoundedContextConfigurationProvider;

        public AccountancyBoundedContextConfigurator(IConfiguration configuration, IServiceCollection services, IBoundedContextConfigurationProvider boundedContextConfigurationProvider)
            : base(configuration, services)
        {
            //var section = configuration.GetSection("Merp:Accountancy:InvoicingSettings");
            var config = new InvoicingSettings();
            //new ConfigureFromConfigurationOptions<InvoicingSettings>(section)
            //    .Configure(config);
            services.AddSingleton(config);

            BoundedContextConfigurationProvider = boundedContextConfigurationProvider ?? throw new System.ArgumentNullException(nameof(boundedContextConfigurationProvider));
        }

        protected override void ConfigureEventStore()
        {
            var mongoDbConnectionString = BoundedContextConfigurationProvider.GetEventStoreConnectionString();
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
            Services.AddTransient<IEventStore, MementoFX.Persistence.MongoDB.MongoDbEventStore>();
            Services.AddTransient<IRepository, MementoFX.Persistence.Repository>();
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
            var readModelConnectionString = BoundedContextConfigurationProvider.GetReadModelConnectionString();
            Services.AddDbContext<Merp.Accountancy.QueryStack.AccountancyDbContext>(options => options.UseSqlServer(readModelConnectionString));
            Services.AddScoped<Merp.Accountancy.QueryStack.IDatabase, Merp.Accountancy.QueryStack.Database>();

            var draftsConnectionString = BoundedContextConfigurationProvider.GetDraftsConnectionString();
            Services.AddDbContext<Merp.Accountancy.Drafts.AccountancyDraftsDbContext>(options => options.UseSqlServer(draftsConnectionString));
            Services.AddScoped<Merp.Accountancy.Drafts.IDatabase, Merp.Accountancy.Drafts.Database>();
            Services.AddScoped<Merp.Accountancy.Drafts.Commands.OutgoingInvoiceCommands>();
            Services.AddScoped<Merp.Accountancy.Drafts.Commands.OutgoingCreditNoteCommands>();
        }

        protected override void RegisterAclServices()
        {
            //Acl Services
        }

        public class InvoicingSettings
        {
            public string CompanyName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string TaxId { get; set; }
        }
    }
}
