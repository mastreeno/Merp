using Merp.Sales.CommandStack.Events;
using Merp.Sales.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Sales.QueryStack.Denormalizers
{
    public class BusinessProposalDenormalizer : 
        IHandleMessages<ProjectRegisteredEvent>,
        IHandleMessages<ProjectCompletedEvent>
    {
        private DbContextOptions<SalesDbContext> Options;

        public BusinessProposalDenormalizer(DbContextOptions<SalesDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(ProjectRegisteredEvent message)
        {
            var jobOrder = new BusinessProposal();
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

            using (var db = new SalesDbContext(Options))
            {
                db.Proposals.Add(jobOrder);
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

        public async Task Handle(ProjectCompletedEvent message)
        {
            using (var db = new SalesDbContext(Options))
            {
                var jobOrder = db.Proposals
                    .OfType<BusinessProposal>()
                    .Where(jo => jo.Id == message.ProjectId)
                    .Single();
                jobOrder.DateOfCompletion = message.DateOfCompletion;
                jobOrder.IsCompleted = true;
                await db.SaveChangesAsync();
            }
        }
    }
}
