using MementoFX.Persistence;
using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.CommandStack.Model;
using Merp.Accountancy.QueryStack;
using Merp.Accountancy.Web.Models.JobOrder;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.WorkerServices
{
    public class JobOrderControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public IRepository Repository { get; private set; }

        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public JobOrderControllerWorkerServices(IDatabase database, IRepository repository, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public SearchModel SearchJobOrders(bool currentOnly, Guid? customerId, string jobOrderName, int page, int size)
        {
            var query = from jo in Database.JobOrders
                        orderby jo.CustomerName, jo.Name
                        select new SearchModel.JobOrderDescriptor
                        {
                            CustomerId = jo.CustomerId,
                            CustomerName = jo.CustomerName,
                            IsCompleted = jo.IsCompleted,
                            Name = jo.Name,
                            Number = jo.Number,
                            Id = jo.Id,
                            OriginalId = jo.OriginalId
                        };

            if (currentOnly)
                query = query.Where(jo => jo.IsCompleted == false);
            if (customerId.HasValue)
                query = query.Where(jo => jo.CustomerId == customerId.Value);
            if (!string.IsNullOrEmpty(jobOrderName))
                query = query.Where(jo => jo.Name.Contains(jobOrderName));

            int totalNumberOfJobOrders = query.Count();

            int skip = (page - 1) * size;

            return new SearchModel
            {
                JobOrders = query.Skip(skip).Take(size),
                TotalNumberOfJobOrders = totalNumberOfJobOrders
            };
        }

        public IEnumerable<JobOrderCustomerModel> GetJobOrderCustomers()
        {
            var customers = (from jo in Database.JobOrders
                             orderby jo.CustomerName
                             select new JobOrderCustomerModel
                             {
                                 Id = jo.CustomerId,
                                 Name = jo.CustomerName
                             }).Distinct().ToList();

            return customers;
        }

        public DetailModel GetJobOrderDetail(Guid jobOrderId)
        {
            var jobOrder = Repository.GetById<JobOrder>(jobOrderId);

            var model = new DetailModel();
            model.ManagerId = jobOrder.ManagerId;
            model.CustomerId = jobOrder.CustomerId;
            model.ContactPersonId = jobOrder.ContactPersonId;
            model.DateOfStart = jobOrder.DateOfStart;
            model.DueDate = jobOrder.DueDate;
            model.JobOrderId = jobOrder.Id;
            model.JobOrderNumber = jobOrder.Number;
            model.JobOrderName = jobOrder.Name;
            model.Description = jobOrder.Description;
            model.Price = jobOrder?.Price?.Amount ?? 0;
            model.IsCompleted = jobOrder.IsCompleted;
            model.Balance = jobOrder.Balance;

            return model;
        }

        public async Task CreateJobOrderAsync(CreateModel model)
        {
            var userId = GetCurrentUserId();
            var command = new RegisterJobOrderCommand(
                    userId,
                    model.Customer.OriginalId,
                    model.Customer.Name,
                    model.ContactPerson.OriginalId,
                    model.Manager.OriginalId,
                    model.Price.Amount,
                    model.Price.Currency,
                    model.DateOfStart,
                    model.DueDate,
                    model.IsTimeAndMaterial,
                    model.Name,
                    model.PurchaseOrderNumber,
                    model.Description
                );

            await Bus.Send(command);
        }

        public IEnumerable<BalanceModel> GetBalanceViewModel(Guid jobOrderId, DateTime dateFrom, DateTime dateTo, BalanceModel.Scale scale)
        {
            DateTime[] dates = null;
            switch (scale)
            {
                case BalanceModel.Scale.Daily:
                case BalanceModel.Scale.Weekly:
                    var step = scale == BalanceModel.Scale.Weekly ? 7 : 1;
                    dates = EachDay(dateFrom, dateTo, step).ToArray();
                    break;
                case BalanceModel.Scale.Monthly:
                    dates = EachMonth(dateFrom, dateTo).ToArray();
                    break;
                case BalanceModel.Scale.Quarterly:
                    dates = EachQuarter(dateFrom, dateTo).ToArray();
                    break;
                case BalanceModel.Scale.Yearly:
                    dates = EachYear(dateFrom, dateTo).ToArray();
                    break;
            }

            var isTimeAndMaterial = (from jo in Database.JobOrders
                                     where jo.OriginalId == jobOrderId
                                     select jo.IsTimeAndMaterial).Single();

            var jobOrders = Repository.GetSeriesById<JobOrder>(jobOrderId, dates);

            var model = new List<BalanceModel>();
            for (int i = 0; i < dates.Count(); i++)
            {
                var balance = new BalanceModel()
                {
                    Date = dates[i],
                    Balance = jobOrders[i].Balance
                };
                model.Add(balance);
            }

            return model;
        }

        public async Task ExtendJobOrderAsync(Guid jobOrderId, ExtendModel model)
        {
            var userId = GetCurrentUserId();
            var command = new ExtendJobOrderCommand(userId, jobOrderId, model.NewDueDate, model.Price);
            await Bus.Send(command);
        }

        public async Task MarkJobOrderAsCompletedAsync(Guid jobOrderId, MarkAsCompletedModel model)
        {
            var userId = GetCurrentUserId();
            var command = new MarkJobOrderAsCompletedCommand(userId, jobOrderId, model.DateOfCompletion);
            await Bus.Send(command);
        }

        public decimal GetEvaluateJobOrderBalance(Guid jobOrderId)
        {
            var jobOrder = Repository.GetById<JobOrder>(jobOrderId, DateTime.Now);
            var balance = jobOrder.Balance;
            return balance;
        }
        
        public IncomingCreditNotesAssociatedToJobOrderModel GetIncomingCreditNotesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = new IncomingCreditNotesAssociatedToJobOrderModel();
            model.IncomingCreditNotes = (from c in Database.IncomingCreditNotes.PerJobOrder(jobOrderId)
                                         orderby c.Date
                                         select new IncomingCreditNotesAssociatedToJobOrderModel.CreditNote
                                         {
                                             Currency = c.Currency,
                                             DateOfIssue = c.Date,
                                             Number = c.Number,
                                             Price = c.TotalPrice,
                                             SupplierName = c.Supplier.Name
                                         }).ToArray();

            return model;
        }

        public IncomingInvoicesAssociatedToJobOrderModel GetIncomingInvoicesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = new IncomingInvoicesAssociatedToJobOrderModel();
            model.IncomingInvoices = (from i in Database.IncomingInvoices.PerJobOrder(jobOrderId)
                                      orderby i.Date
                                      select new IncomingInvoicesAssociatedToJobOrderModel.Invoice
                                      {
                                          DateOfIssue = i.Date,
                                          Price = i.TotalPrice,
                                          Currency = i.Currency,
                                          Number = i.Number,
                                          SupplierName = i.Supplier.Name
                                      }).ToArray();
            return model;
        }

        public OutgoingCreditNotesAssociatedToJobOrderModel GetOutgoingCreditNotesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = new OutgoingCreditNotesAssociatedToJobOrderModel();
            model.OutgoingCreditNotes = (from c in Database.OutgoingCreditNotes.PerJobOrder(jobOrderId)
                                         orderby c.Date
                                         select new OutgoingCreditNotesAssociatedToJobOrderModel.CreditNote
                                         {
                                             Currency = c.Currency,
                                             CustomerName = c.Customer.Name,
                                             DateOfIssue = c.Date,
                                             Number = c.Number,
                                             Price = c.TotalPrice
                                         }).ToArray();

            return model;
        }

        public OutgoingInvoicesAssociatedToJobOrderModel GetOutgoingInvoicesAssociatedToJobOrder(Guid jobOrderId)
        {
            var model = new OutgoingInvoicesAssociatedToJobOrderModel();
            model.OutgoingInvoices = (from i in Database.OutgoingInvoices.PerJobOrder(jobOrderId)
                                      orderby i.Date
                                      select new OutgoingInvoicesAssociatedToJobOrderModel.Invoice
                                      {
                                          DateOfIssue = i.Date,
                                          Price = i.TotalPrice,
                                          Currency = i.Currency,
                                          Number = i.Number,
                                          CustomerName = i.Customer.Name
                                      }).ToArray();
            return model;
        }

        #region Private methods
        private Guid GetCurrentUserId()
        {
            var userId = ContextAccessor.HttpContext.User.FindFirstValue("sub");
            return Guid.Parse(userId);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            return EachDay(from, thru, 1);
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru, int step)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(step))
                yield return day;
        }

        private IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
                yield return new DateTime(day.Year, day.Month, 1);
        }

        private IEnumerable<DateTime> EachQuarter(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(3))
                yield return new DateTime(day.Year, day.Month, 1);
        }

        private IEnumerable<DateTime> EachYear(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddYears(1))
                yield return new DateTime(day.Year, 12, 31);
        }
        #endregion
    }
}
