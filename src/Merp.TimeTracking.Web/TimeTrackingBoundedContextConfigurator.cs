using MementoFX.Persistence;
using Merp.TimeTracking.TaskManagement.CommandStack.Events;
using Merp.TimeTracking.TaskManagement.CommandStack.Sagas;
using Merp.TimeTracking.TaskManagement.QueryStack.Denormalizers;
using Merp.TimeTracking.Web.Areas.TaskManagement.WorkerServices;
using Merp.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.ServiceProvider;

namespace Merp.TimeTracking.Web
{
    public class TimeTrackingBoundedContextConfigurator : BoundedContextConfigurator
    {
        public TimeTrackingBoundedContextConfigurator(IConfiguration configuration, IServiceCollection services) : base(configuration, services)
        {

        }

        protected override void ConfigureEventStore()
        {
            var mongoDbConnectionString = Configuration.GetConnectionString("Merp-TimeTracking-EventStore");
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
            Services.AddTransient<IEventStore, MementoFX.Persistence.MongoDB.MongoDbEventStore>();
            Services.AddTransient<IRepository, MementoFX.Persistence.Repository>();
        }

        protected override void SubscribeEvents()
        {
            //Events
            Bus.Subscribe<TaskAddedEvent>();
            Bus.Subscribe<TaskCancelledEvent>();
            Bus.Subscribe<TaskCompletedEvent>();
            Bus.Subscribe<TaskUpdatedEvent>();
        }

        protected override void RegisterDenormalizers()
        {
            //Denormalizers
            Services.AutoRegisterHandlersFromAssemblyOf<TaskEvents>();
        }

        protected override void RegisterHandlers()
        {
            //Handlers
        }

        protected override void RegisterSagas()
        {
            //Sagas
            Services.AutoRegisterHandlersFromAssemblyOf<TaskLifecycleSaga>();
        }

        protected override void RegisterServices()
        {
            
        }

        protected override void RegisterTypes()
        {
            //Types
            var readModelConnectionString = Configuration.GetConnectionString("Merp-TimeTracking-ReadModel");
            Services.AddDbContext<Merp.TimeTracking.TaskManagement.QueryStack.TaskManagementDbContext>(options => options.UseSqlServer(readModelConnectionString));
            Services.AddScoped<Merp.TimeTracking.TaskManagement.QueryStack.IDatabase, Merp.TimeTracking.TaskManagement.QueryStack.Database>();
        }

        protected override void RegisterWorkerServices()
        {
            //Worker Services
            Services.AddScoped<TaskControllerWorkerServices>();
        }

        protected override void RegisterAclServices()
        {
        }
    }
}
