using Merp.ProjectManagement.CommandStack.Events;
using Merp.Web;
using Merp.Web.Site.Areas.ProjectManagement.WorkerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.ProjectManagement.Web.Areas.PM
{
    public class ProjectManagementBoundedContextConfigurator : BoundedContextConfigurator
    {
        public ProjectManagementBoundedContextConfigurator(IConfigurationRoot configuration, IServiceCollection services) : base(configuration, services)
        {
            var section = configuration.GetSection("Merp:Accountancy:InvoicingSettings");
            
            //new ConfigureFromConfigurationOptions<InvoicingSettings>(section)
            //    .Configure(config);
            //services.AddSingleton(config);
        }

        protected override void ConfigureEventStore()
        {
            
        }

        protected override void RegisterAclServices()
        {
            
        }

        protected override void RegisterDenormalizers()
        {
            
        }

        protected override void RegisterHandlers()
        {
            
        }

        protected override void RegisterSagas()
        {
            
        }

        protected override void RegisterServices()
        {
            
        }

        protected override void RegisterTypes()
        {
            
        }

        protected override void RegisterWorkerServices()
        {
            Services.AddScoped<HomeControllerWorkerServices, HomeControllerWorkerServices>();
        }

        protected override void SubscribeEvents()
        {
            //Events
            Bus.Subscribe<ProjectCompletedEvent>();
            Bus.Subscribe<ProjectExtendedEvent>();
            Bus.Subscribe<ProjectRegisteredEvent>();
        }
    }
}
