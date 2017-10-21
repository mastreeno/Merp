using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class JobOrderDenormalizer : 
        IHandleMessages<JobOrderRegisteredEvent>,
        IHandleMessages<JobOrderExtendedEvent>,
        IHandleMessages<JobOrderCompletedEvent>
    {
        private DbContextOptions<AccountancyDbContext> Options;

        public JobOrderDenormalizer(DbContextOptions<AccountancyDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(JobOrderRegisteredEvent message)
        {
            var jobOrder = new JobOrder();
            jobOrder.OriginalId = message.JobOrderId;
            jobOrder.CustomerId = message.CustomerId;
            jobOrder.CustomerName = message.CustomerName;
            jobOrder.Description = message.Description;
            jobOrder.ManagerId = message.ManagerId;
            jobOrder.DateOfRegistration = message.DateOfRegistration;
            jobOrder.DateOfStart = message.DateOfStart;
            jobOrder.DueDate = message.DueDate;
            jobOrder.Name = message.JobOrderName;
            jobOrder.Number = message.JobOrderNumber;
            jobOrder.Price = message.Price;
            jobOrder.Currency = message.Currency;
            jobOrder.PurchaseOrderNumber = message.PurchaseOrderNumber;
            jobOrder.IsCompleted = false;
            jobOrder.IsTimeAndMaterial = false;

            using (var db = new AccountancyDbContext(Options))
            {
                db.JobOrders.Add(jobOrder);
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

        public async Task Handle(JobOrderExtendedEvent message)
        {
            using (var db = new AccountancyDbContext(Options))
            {
                var jobOrder = db.JobOrders.OfType<JobOrder>().Where(jo => jo.OriginalId == message.JobOrderId).Single();
                jobOrder.DueDate = message.NewDueDate;
                jobOrder.Price = message.Price;
                await db.SaveChangesAsync();
            }
            
        }

        public async Task Handle(JobOrderCompletedEvent message)
        {
            using (var db = new AccountancyDbContext(Options))
            {
                var jobOrder = db.JobOrders
                    .OfType<JobOrder>()
                    .Where(jo => jo.OriginalId == message.JobOrderId)
                    .Single();
                jobOrder.DateOfCompletion = message.DateOfCompletion;
                jobOrder.IsCompleted = true;
                await db.SaveChangesAsync();
            }
        }
    }
}
