using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.Web
{
    public abstract class BoundedContextConfigurator
    {
        public IBus Bus { get; private set; }
        public IWebHostEnvironment Environment { get; private set; }
        public IServiceCollection Services { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected abstract void RegisterDenormalizers();
        protected abstract void RegisterHandlers();
        protected abstract void RegisterSagas();
        protected abstract void RegisterServices();
        protected abstract void SubscribeEvents();
        protected abstract void RegisterTypes();
        protected abstract void RegisterWorkerServices();
        protected abstract void RegisterAclServices();
        protected abstract void ConfigureEventStore();

        public BoundedContextConfigurator(IConfiguration configuration, IServiceCollection services)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Bus = services.BuildServiceProvider().GetService<IBus>();
            Environment = services.BuildServiceProvider().GetService<IWebHostEnvironment>();
        }

        public void Configure()
        {
            if (Environment.IsDevelopment() ||
                Environment.IsOnPremises() ||
                Environment.IsAzureCosmosDB() ||
                Environment.IsAzureMongoDB())
            {
                RegisterDenormalizers();
                RegisterHandlers();
                RegisterSagas();
                RegisterServices();
                SubscribeEvents();
            }
            RegisterTypes();
            RegisterWorkerServices();
            RegisterAclServices();
            ConfigureEventStore();
        }
    }
}
