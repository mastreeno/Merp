using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using System.Linq;
using Rebus.Handlers;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class TimeAndMaterialJobOrderDenormalizer : 
        IHandleMessages<TimeAndMaterialJobOrderRegisteredEvent>, 
        IHandleMessages<TimeAndMaterialJobOrderExtendedEvent>,
        IHandleMessages<TimeAndMaterialJobOrderCompletedEvent>
    {
        public async Task Handle(TimeAndMaterialJobOrderRegisteredEvent message)
        {
            var timeAndMaterialJobOrder = new TimeAndMaterialJobOrder();
                timeAndMaterialJobOrder.OriginalId = message.JobOrderId;
                timeAndMaterialJobOrder.CustomerId = message.CustomerId;
                timeAndMaterialJobOrder.Description = message.Description;
                timeAndMaterialJobOrder.ManagerId = message.ManagerId;
                timeAndMaterialJobOrder.DateOfStart = message.DateOfStart;
                timeAndMaterialJobOrder.DateOfExpiration = message.DateOfExpiration;
                timeAndMaterialJobOrder.Name = message.JobOrderName;
                timeAndMaterialJobOrder.Number = message.JobOrderNumber;
                timeAndMaterialJobOrder.PurchaseOrderNumber = message.PurchaseOrderNumber;
                timeAndMaterialJobOrder.Currency = message.Currency;
                timeAndMaterialJobOrder.Value = message.Value;
                timeAndMaterialJobOrder.IsCompleted = false;
                timeAndMaterialJobOrder.IsTimeAndMaterial = true;
                timeAndMaterialJobOrder.IsFixedPrice = false;

            using (var db = new AccountancyContext())
            {
                db.JobOrders.Add(timeAndMaterialJobOrder);
                await db.SaveChangesAsync();
            }
        }

        public async Task Handle(TimeAndMaterialJobOrderExtendedEvent message)
        {
            using (var db = new AccountancyContext())
            {
                var jobOrder = db.JobOrders.OfType<TimeAndMaterialJobOrder>().Where(jo => jo.OriginalId == message.JobOrderId).Single();
                jobOrder.DateOfExpiration = message.NewDateOfExpiration;
                jobOrder.Value = message.Value;
                await db.SaveChangesAsync();
            }
        }

        public async Task Handle(TimeAndMaterialJobOrderCompletedEvent message)
        {
            using (var db = new AccountancyContext())
            {
                var jobOrder = db.JobOrders.OfType<TimeAndMaterialJobOrder>().Where(jo => jo.OriginalId == message.JobOrderId).Single();
                jobOrder.DateOfCompletion = message.DateOfCompletion;
                jobOrder.IsCompleted = true;
                await db.SaveChangesAsync();
            }
        }
    }
}
