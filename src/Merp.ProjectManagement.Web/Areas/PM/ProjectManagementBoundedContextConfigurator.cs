using Merp.Web;
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
            throw new NotImplementedException();
        }

        protected override void RegisterAclServices()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterDenormalizers()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterHandlers()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterSagas()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterServices()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterTypes()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterWorkerServices()
        {
            throw new NotImplementedException();
        }

        protected override void SubscribeEvents()
        {
            throw new NotImplementedException();
        }
    }
}
