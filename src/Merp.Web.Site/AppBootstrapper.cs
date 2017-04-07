using System;
using Memento.Messaging;
using Memento.Messaging.Rebus;
using Memento.Persistence;
using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.CommandStack.Sagas;
using Merp.Accountancy.CommandStack.Services;
using Merp.Accountancy.QueryStack.Denormalizers;
using Merp.Registry.CommandStack.Events;
using Merp.Registry.CommandStack.Sagas;
using Merp.Registry.QueryStack.Denormalizers;
using Merp.Web.Site.Areas.Accountancy.WorkerServices;
using Merp.Web.Site.Areas.Registry.WorkerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Security.Authentication;

namespace Merp.Web.Site
{
    public class AppBootstrapper
    {
        public IServiceCollection Services { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }
        public IHostingEnvironment Environment { get; set; }

        public AppBootstrapper(IConfigurationRoot configuration, IServiceCollection services)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (services == null)
                throw new ArgumentNullException(nameof(services));
       
            Configuration = configuration;
            Services = services;
            Environment = Services.BuildServiceProvider().GetService<IHostingEnvironment>();
        }

        public void Configure()
        {
            var env = Services.BuildServiceProvider().GetService<IHostingEnvironment>();
            ConfigureBus();

            var bus = Services.BuildServiceProvider().GetService<IBus>();
            new AccountancyBoundedContext(Configuration, Services).Configure();
            new RegistryBoundedContext(Configuration, Services).Configure();
        }

        private void ConfigureBus()
        {
            var config = Rebus.Config.Configure.With(new NetCoreServiceCollectionContainerAdapter(Services))
                .Logging(l => l.Trace())
                .Routing(r => r.TypeBased()
                    .MapAssemblyOf<IncomingInvoiceSaga>(Configuration["Rebus:QueueName"])
                    .MapAssemblyOf<CompanySaga>(Configuration["Rebus:QueueName"])
                )
                .Subscriptions(s => s.StoreInSqlServer(Configuration["Rebus:Subscriptions:ConnectionString"], Configuration["Rebus:Subscriptions:TableName"], isCentralized: true))
                .Sagas(s => s.StoreInSqlServer(Configuration["Rebus:Sagas:ConnectionString"], Configuration["Rebus:Sagas:MessagesTableName"], Configuration["Rebus:Sagas:IndexesTableName"]))
                .Timeouts(t => t.StoreInSqlServer(Configuration["Rebus:Timeouts:ConnectionString"], Configuration["Rebus:Timeouts:TableName"], true));
            if (Environment.IsDevelopment() || Environment.IsOnPremises())
            {
                config.Transport(t => t.UseMsmq(Configuration["Rebus:QueueName"]));
            }
            else if (Environment.IsAzure())
            {
                config.Transport(t => t.UseAzureServiceBus(Configuration["Rebus:ServiceBusConnectionString"], Configuration["Rebus:QueueName"], AzureServiceBusMode.Basic));
            }
            else
            {
                throw new InvalidOperationException("Unknown execution environment");
            }
            var bus = config.Start();
            Services.AddSingleton(bus);
            Services.AddTransient<IEventDispatcher, RebusEventDispatcher>();
        }

        public class AccountancyBoundedContext
        {
            public IBus Bus { get; set; }
            public IHostingEnvironment Environment { get; set; }
            public IServiceCollection Services { get; set; }

            public IConfigurationRoot Configuration { get; private set; }

            public AccountancyBoundedContext(IConfigurationRoot configuration, IServiceCollection services)
            {
                if (services == null)
                    throw new ArgumentNullException(nameof(services));

                if (configuration == null)
                    throw new ArgumentNullException(nameof(configuration));

                Bus = services.BuildServiceProvider().GetService<IBus>();
                Configuration = configuration;             
                Services = services;
                Environment = services.BuildServiceProvider().GetService<IHostingEnvironment>();
            }

            public void Configure()
            {
                if(Environment.IsDevelopment() || Environment.IsOnPremises())
                {
                    RegisterDenormalizers();
                    RegisterHandlers();
                    RegisterSagas();
                    RegisterServices();
                    SubscribeEvents();
                }
                RegisterTypes();
                RegisterWorkerServices();
                ConfigureEventStore();
            }

            private void ConfigureEventStore()
            {
                if (Environment.IsDevelopment() || Environment.IsOnPremises())
                {
                    var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Accountancy-EventStore");
                    var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
                    var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
                    Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
                    Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
                    Services.AddTransient<IRepository, Memento.Persistence.Repository>();
                }
                else if (Environment.IsAzure())
                {
                    var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Accountancy-EventStore");
                    var mongoDbUrl = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString);
                    MongoClientSettings settings = new MongoClientSettings();
                    settings.Server = new MongoServerAddress(mongoDbUrl.Server.Host, mongoDbUrl.Server.Port);
                    settings.UseSsl = true;
                    settings.SslSettings = new SslSettings();
                    settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

                    MongoIdentity identity = new MongoInternalIdentity(mongoDbUrl.DatabaseName, mongoDbUrl.Username);
                    MongoIdentityEvidence evidence = new PasswordEvidence(mongoDbUrl.Password);

                    settings.Credentials = new List<MongoCredential>()
                {
                    new MongoCredential("SCRAM-SHA-1", identity, evidence)
                };

                    MongoClient client = new MongoClient(settings);
                    Services.AddSingleton(client.GetDatabase(mongoDbUrl.DatabaseName));
                    Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
                    Services.AddTransient<IRepository, Memento.Persistence.Repository>();
                }
                else
                {
                    throw new InvalidOperationException("Unknown execution environment");
                }
            }

            private void SubscribeEvents()
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

            private void RegisterDenormalizers()
            {
                //Denormalizers
                Services.AutoRegisterHandlersFromAssemblyOf<FixedPriceJobOrderDenormalizer>();
            }

            private void RegisterHandlers()
            {
                //Handlers
            }

            private void RegisterSagas()
            {
                //Sagas
                Services.AutoRegisterHandlersFromAssemblyOf<FixedPriceJobOrderSaga>();
            }

            private void RegisterServices()
            {
                //Services
                Services.AddScoped<IJobOrderNumberGenerator, JobOrderNumberGenerator>();
                Services.AddScoped<IOutgoingInvoiceNumberGenerator, OutgoingInvoiceNumberGenerator>();
            }

            private void RegisterTypes()
            {
                //Types
                var readModelConnectionString = Configuration.GetConnectionString("Merp-Accountancy-ReadModel");

                Services.AddScoped<Merp.Accountancy.QueryStack.AccountancyContext>((s) => new Merp.Accountancy.QueryStack.AccountancyContext(readModelConnectionString));
                Services.AddScoped<Merp.Accountancy.QueryStack.IDatabase, Merp.Accountancy.QueryStack.Database>((s) => new Merp.Accountancy.QueryStack.Database(readModelConnectionString));
            }

            private void RegisterWorkerServices()
            {
                //Worker Services
                Services.AddScoped<InvoiceControllerWorkerServices, InvoiceControllerWorkerServices>();
                Services.AddScoped<JobOrderControllerWorkerServices, JobOrderControllerWorkerServices>();
            }
        }
        
        public class RegistryBoundedContext
        {
            public IBus Bus { get; set; }
            public IHostingEnvironment Environment { get; set; }
            public IServiceCollection Services { get; set; }

            public IConfigurationRoot Configuration { get; private set; }

            public RegistryBoundedContext(IConfigurationRoot configuration, IServiceCollection services)
            {
                if (services == null)
                    throw new ArgumentNullException(nameof(services));

                if (configuration == null)
                    throw new ArgumentNullException(nameof(configuration));

                Bus = services.BuildServiceProvider().GetService<IBus>();
                Configuration = configuration;
                Environment = services.BuildServiceProvider().GetService<IHostingEnvironment>();
                Services = services;
            }

            public void Configure()
            {
                if (Environment.IsDevelopment() || Environment.IsOnPremises())
                {
                    RegisterDenormalizers();
                    RegisterHandlers();
                    RegisterSagas();
                    RegisterServices();
                    SubscribeEvents();
                }
                RegisterTypes();
                RegisterWorkerServices();
                ConfigureEventStore();
            }

            private void ConfigureEventStore()
            {
                if (Environment.IsDevelopment() || Environment.IsOnPremises())
                {
                    var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Registry-EventStore");
                    var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
                    var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
                    Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
                    Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
                    Services.AddTransient<IRepository, Memento.Persistence.Repository>();
                }
                else if (Environment.IsAzure())
                {
                    var mongoDbConnectionString = Configuration.GetConnectionString("Merp-Registry-EventStore");
                    var mongoDbUrl = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString);
                    MongoClientSettings settings = new MongoClientSettings();
                    settings.Server = new MongoServerAddress(mongoDbUrl.Server.Host, mongoDbUrl.Server.Port);
                    settings.UseSsl = true;
                    settings.SslSettings = new SslSettings();
                    settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

                    MongoIdentity identity = new MongoInternalIdentity(mongoDbUrl.DatabaseName, mongoDbUrl.Username);
                    MongoIdentityEvidence evidence = new PasswordEvidence(mongoDbUrl.Password);

                    settings.Credentials = new List<MongoCredential>()
                {
                    new MongoCredential("SCRAM-SHA-1", identity, evidence)
                };

                    MongoClient client = new MongoClient(settings);
                    Services.AddSingleton(client.GetDatabase(mongoDbUrl.DatabaseName));
                    Services.AddTransient<IEventStore, Memento.Persistence.MongoDB.MongoDbEventStore>();
                    Services.AddTransient<IRepository, Memento.Persistence.Repository>();
                }
                else
                {
                    throw new InvalidOperationException("Unknown execution environment");
                }
            }

            private void SubscribeEvents()
            {
                //Events
                Bus.Subscribe<CompanyNameChangedEvent>();
                Bus.Subscribe<CompanyRegisteredEvent>();
                Bus.Subscribe<PersonRegisteredEvent>();
            }

            private void RegisterDenormalizers()
            {
                //Denormalizers
                Services.AutoRegisterHandlersFromAssemblyOf<CompanyDenormalizer>();
            }

            private void RegisterHandlers()
            {
                //Handlers
            }

            private void RegisterSagas()
            {
                //Sagas
                Services.AutoRegisterHandlersFromAssemblyOf<CompanySaga>();
            }

            private void RegisterServices()
            {

            }

            private void RegisterTypes()
            {
                //Types
                var readModelConnectionString = Configuration.GetConnectionString("Merp-Registry-ReadModel");

                Services.AddScoped<Merp.Registry.QueryStack.RegistryDbContext>((s) => new Merp.Registry.QueryStack.RegistryDbContext(readModelConnectionString));
                Services.AddScoped<Merp.Registry.QueryStack.IDatabase, Merp.Registry.QueryStack.Database>((s) => new Merp.Registry.QueryStack.Database(readModelConnectionString));
            }

            private void RegisterWorkerServices()
            {
                //Worker Services
                Services.AddScoped<PersonControllerWorkerServices>();
                Services.AddScoped<PartyControllerWorkerServices>();
                Services.AddScoped<CompanyControllerWorkerServices>();
            }
        }
    }

    static class EnvironmentExtensions
    {
        public static bool IsAzure(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("Azure");
        }

        public static bool IsOnPremises(this IHostingEnvironment env)
        {
            return env.EnvironmentName.Contains("OnPremises");
        }
    }
}