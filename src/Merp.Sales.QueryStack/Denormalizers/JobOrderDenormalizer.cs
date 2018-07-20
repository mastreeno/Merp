using Merp.Sales.CommandStack.Events;
using Merp.Sales.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Sales.QueryStack.Denormalizers
{
    public class JobOrderDenormalizer : 
        IHandleMessages<ProjectRegisteredEvent>,
        IHandleMessages<ProjectExtendedEvent>,
        IHandleMessages<ProjectCompletedEvent>
    {
        private DbContextOptions<ProjectManagementDbContext> Options;

        public JobOrderDenormalizer(DbContextOptions<ProjectManagementDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(ProjectRegisteredEvent message)
        {
            var jobOrder = new Project();
            jobOrder.Id = message.ProjectId;
            jobOrder.CustomerId = message.CustomerId;
            jobOrder.ContactPersonId = message.ContactPersonId;
            jobOrder.ManagerId = message.ManagerId;
            jobOrder.DateOfRegistration = message.DateOfRegistration;
            jobOrder.DateOfStart = message.DateOfStart;
            jobOrder.DueDate = message.DueDate;
            jobOrder.Description = message.Description;
            jobOrder.Name = message.ProjectName;
            jobOrder.Number = message.ProjectNumber;
            jobOrder.Price = message.Price;
            jobOrder.Currency = message.Currency;
            jobOrder.CustomerPurchaseOrderNumber = message.CustomerPurchaseOrderNumber;
            jobOrder.IsCompleted = false;
            jobOrder.IsTimeAndMaterial = false;

            using (var db = new ProjectManagementDbContext(Options))
            {
                db.Projects.Add(jobOrder);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }

        public async Task Handle(ProjectExtendedEvent message)
        {
            using (var db = new ProjectManagementDbContext(Options))
            {
                var jobOrder = db.Projects.OfType<Project>().Where(jo => jo.Id == message.ProjectId).Single();
                jobOrder.DueDate = message.NewDueDate;
                jobOrder.Price = message.Price;
                await db.SaveChangesAsync();
            }
            
        }

        public async Task Handle(ProjectCompletedEvent message)
        {
            using (var db = new ProjectManagementDbContext(Options))
            {
                var jobOrder = db.Projects
                    .OfType<Project>()
                    .Where(jo => jo.Id == message.ProjectId)
                    .Single();
                jobOrder.DateOfCompletion = message.DateOfCompletion;
                jobOrder.IsCompleted = true;
                await db.SaveChangesAsync();
            }
        }
    }
}
