using MementoFX.Persistence;
using Merp.Web.Site.Areas.OnTime.WorkerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Merp.TimeTracking.TaskManagement.CommandStack.Events;
using Merp.TimeTracking.TaskManagement.CommandStack.Sagas;
using Merp.TimeTracking.TaskManagement.QueryStack;
using Merp.TimeTracking.TaskManagement.QueryStack.Denormalizers;
using Rebus.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace Merp.Web.Site.Areas.OnTime
{
    public class OnTimeBoundedContextConfigurator : BoundedContextConfigurator
    {
        public OnTimeBoundedContextConfigurator(IConfigurationRoot configuration, IServiceCollection services) : base(configuration, services)
        {

        }

        //protected override void ConfigureReadModel()
        //{

        //}

        protected override void RegisterSagas()
        {
            Services.AutoRegisterHandlersFromAssemblyOf<TaskLifecycleSaga>();
        }

        protected override void RegisterDenormalizers()
        {
            Services.AutoRegisterHandlersFromAssemblyOf<TaskEvents>();
        }

        protected override void RegisterHandlers()
        {

        }

        protected override void RegisterServices()
        {
            
        }

        protected override void SubscribeEvents()
        {
            Bus.Subscribe<TaskCancelledEvent>();
            Bus.Subscribe<TaskCompletedEvent>();
            Bus.Subscribe<TaskAddedEvent>();
            Bus.Subscribe<TaskUpdatedEvent>();
        }

        protected override void RegisterTypes()
        {
            var readModelConnectionString = Configuration.GetConnectionString("Merp-TimeTracking-ReadModel");
            Services.AddDbContext<TaskManagementDbContext>(options => options.UseSqlServer(readModelConnectionString));
            Services.AddScoped<IDatabase, Database>();
        }

        protected override void RegisterWorkerServices()
        {
            Services.AddScoped<TaskControllerWorkerServices>();
        }

        protected override void RegisterAclServices()
        {
            //Acl Services
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
    }
}
