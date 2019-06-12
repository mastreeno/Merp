using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.QueryStack;
using Merp.Accountancy.Web.Api.Public.Models;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Public.WorkerServices
{
    public class JobOrderControllerWorkerServices
    {
        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public IDatabase Database { get; private set; }

        public JobOrderControllerWorkerServices(IDatabase database, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task ExtendJobOrderAsync(ExtendJobOrderModel model)
        {
            var command = new ExtendJobOrderCommand(
                model.UserId,
                model.JobOrderId,
                model.NewDueDate,
                model.Price
            );

            await Bus.Send(command);
        }

        public async Task ImportJobOrderAsync(ImportJobOrderModel model)
        {
            var command = new ImportJobOrderCommand(
                model.UserId,
                model.JobOrderId,
                model.Customer.Id,
                model.Customer.Name,
                model.ManagerId,
                model.Price,
                model.Currency,
                model.DateOfRegistration,
                model.DateOfStart,
                model.DueDate,
                model.IsTimeAndMaterial,
                model.JobOrderNumber,
                model.JobOrderName,
                model.PurchaseOrderNumber,
                model.Description
            );

            await Bus.Send(command);
        }

        public async Task MarkJobOrderAsCompletedAsync(MarkJobOrderAsCompletedModel model)
        {
            var command = new MarkJobOrderAsCompletedCommand(
                model.UserId,
                model.JobOrderId,
                model.DateOfCompletion
            );

            await Bus.Send(command);
        }
    }
}
