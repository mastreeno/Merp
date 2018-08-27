using MementoFX.Persistence;
using Merp.Web.Site.Areas.OnTime.WorkerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OnTime.TaskManagement.CommandStack.Events;
using OnTime.TaskManagement.CommandStack.Sagas;
using OnTime.TaskManagement.QueryStack;
using OnTime.TaskManagement.QueryStack.Denormalizers;
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
            Bus.Subscribe<DueDateRemovedFromTaskEvent>();
            Bus.Subscribe<DueDateSetForTaskEvent>();
            Bus.Subscribe<TaskDeletedEvent>();
            Bus.Subscribe<TaskCompletedEvent>();
            Bus.Subscribe<TaskCreatedEvent>();
            Bus.Subscribe<TaskReactivatedEvent>();
            Bus.Subscribe<TaskRenamedEvent>();
        }

        protected override void RegisterTypes()
        {
            var readModelConnectionString = Configuration.GetConnectionString("OnTime-TaskManagement-ReadModel");
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
            var mongoDbConnectionString = Configuration.GetConnectionString("OnTime-TaskManagement-EventStore");
            var mongoDbDatabaseName = MongoDB.Driver.MongoUrl.Create(mongoDbConnectionString).DatabaseName;
            var mongoClient = new MongoDB.Driver.MongoClient(mongoDbConnectionString);
            Services.AddSingleton(mongoClient.GetDatabase(mongoDbDatabaseName));
            Services.AddTransient<IEventStore, MementoFX.Persistence.MongoDB.MongoDbEventStore>();
            Services.AddTransient<IRepository, MementoFX.Persistence.Repository>();
        }
    }
}
