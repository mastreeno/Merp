using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.QueryStack;
using Merp.Web.Site.Areas.Accountancy.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using Rebus.Bus;

namespace Merp.Web.Site.Areas.Accountancy.WorkerServices
{
    public class InvoiceControllerWorkerServices
    {
        public IBus Bus { get; private set; }
        public IDatabase Database { get; set; }

        public InvoiceControllerWorkerServices(IBus bus, IDatabase database)
        {
            if(bus==null)
                throw new ArgumentNullException(nameof(bus));
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            this.Bus = bus;
            this.Database = database;
        }
        public IssueViewModel GetIssueViewModel()
        {
            var model = new IssueViewModel();
            model.Date = DateTime.Now;
            return model;
        }
        public RegisterViewModel GetRegisterViewModel()
        {
            var model = new RegisterViewModel();
            model.Date = DateTime.Now;
            return model;
        }
        public void Issue(IssueViewModel model)
        {
            var command = new IssueInvoiceCommand(
                model.Date,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,

                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
                );
            Bus.Send(command);
        }
        public void Register(RegisterViewModel model)
        {
            var command = new RegisterIncomingInvoiceCommand(
                model.InvoiceNumber,
                model.Date,
                model.DueDate,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Supplier.OriginalId,
                model.Supplier.Name,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
                );
            Bus.Send(command);
        }

        #region LinkIncomingInvoiceToJobOrder
        public IEnumerable<IncomingInvoicesNotLinkedToAJobOrderViewModel.Invoice> GetListOfIncomingInvoicesNotLinkedToAJobOrderViewModel()
        {
            var model = (from i in Database.IncomingInvoices.NotAssociatedToAnyJobOrder()
                        orderby i.Date
                        select new IncomingInvoicesNotLinkedToAJobOrderViewModel.Invoice
                        {
                            Amount = i.Amount,
                            Number = i.Number,
                            SupplierName = i.Supplier.Name,
                            OriginalId = i.OriginalId
                        }).ToArray();
            return model;
        }

        public LinkIncomingInvoiceToJobOrderViewModel GetLinkIncomingInvoiceToJobOrderViewModel(Guid invoiceId)
        {
            var model = (from i in Database.IncomingInvoices
                        where i.OriginalId == invoiceId
                        select new LinkIncomingInvoiceToJobOrderViewModel
                        {
                            Amount = i.Amount,
                            DateOfLink = i.Date,
                            InvoiceNumber = i.Number,
                            InvoiceOriginalId = i.OriginalId,
                            SupplierName = i.Supplier.Name
                        }).Single();
            return model;
        }

        public void LinkIncomingInvoiceToJobOrder(LinkIncomingInvoiceToJobOrderViewModel model)
        {
            var jobOrderId = (from j in Database.JobOrders
                              where j.Number == model.JobOrderNumber
                                 select j.OriginalId).Single();
            var command = new LinkIncomingInvoiceToJobOrderCommand(model.InvoiceOriginalId, jobOrderId, model.DateOfLink, model.Amount);
            Bus.Send(command);
        }
        #endregion

        #region LinkOutgoingInvoiceToJobOrder
        public IEnumerable<OutgoingInvoicesNotLinkedToAJobOrderViewModel.Invoice> GetListOfOutgoingInvoicesNotLinkedToAJobOrderViewModel()
        {
            var model = (from i in Database.OutgoingInvoices.NotAssociatedToAnyJobOrder()
                         orderby i.Date
                         select new OutgoingInvoicesNotLinkedToAJobOrderViewModel.Invoice
                         {
                             Amount = i.Amount,
                             Number = i.Number,
                             CustomerName = i.Customer.Name,
                             OriginalId = i.OriginalId
                         }).ToArray();
            return model;
        }

        public LinkOutgoingInvoiceToJobOrderViewModel GetLinkOutgoingInvoiceToJobOrderViewModel(Guid invoiceId)
        {
            var model = (from i in Database.OutgoingInvoices
                         where i.OriginalId == invoiceId
                         select new LinkOutgoingInvoiceToJobOrderViewModel
                         {
                             Amount = i.Amount,
                             DateOfLink = i.Date,
                             InvoiceNumber = i.Number,
                             InvoiceOriginalId = i.OriginalId,
                             CustomerName = i.Customer.Name
                         }).Single();
            return model;
        }

        public void LinkOutgoingInvoiceToJobOrder(LinkOutgoingInvoiceToJobOrderViewModel model)
        {
            var jobOrderId = (from j in Database.JobOrders
                              where j.Number == model.JobOrderNumber
                              select j.OriginalId).Single();
            var command = new LinkOutgoingInvoiceToJobOrderCommand(model.InvoiceOriginalId, jobOrderId, model.DateOfLink, model.Amount);
            Bus.Send(command);
        }
        #endregion
    }
}