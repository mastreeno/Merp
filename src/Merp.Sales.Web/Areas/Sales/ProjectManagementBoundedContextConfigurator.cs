using Merp.Sales.CommandStack.Events;
using Merp.Sales.CommandStack.Services;
using Merp.Sales.Web.Areas.Sales.WorkerServices;
using Merp.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Merp.Sales.Web.Areas.Sales
{
    public class ProjectManagementBoundedContextConfigurator : BoundedContextConfigurator
    {
        public ProjectManagementBoundedContextConfigurator(IConfigurationRoot configuration, IServiceCollection services) : base(configuration, services)
        {
            //var section = configuration.GetSection("Merp:Accountancy:InvoicingSettings");
            
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
            Services.AddScoped<IProjectNumberGenerator, ProjectNumberGenerator>();
        }

        protected override void RegisterTypes()
        {
            //Types
            var readModelConnectionString = Configuration.GetConnectionString("Merp-Sales-ReadModel");
            Services.AddDbContext<Merp.Sales.QueryStack.ProjectManagementDbContext>(options => options.UseSqlServer(readModelConnectionString));
            Services.AddScoped<Merp.Sales.QueryStack.IDatabase, Merp.Sales.QueryStack.Database>();
        }

        protected override void RegisterWorkerServices()
        {
            Services.AddScoped<HomeControllerWorkerServices, HomeControllerWorkerServices>();
            Services.AddScoped<ProjectControllerWorkerServices, ProjectControllerWorkerServices>();
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
