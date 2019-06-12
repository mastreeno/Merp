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
    public class InvoiceControllerWorkerServices
    {
        public IBus Bus { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public IDatabase Database { get; private set; }

        public InvoiceControllerWorkerServices(IDatabase database, IBus bus, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        #region Incoming invoices
        public async Task ImportIncomingInvoiceAsync(ImportIncomingInvoiceModel model)
        {
            var command = new ImportIncomingInvoiceCommand(
                model.UserId,
                model.InvoiceId,
                model.InvoiceNumber,
                model.InvoiceDate,
                model.DueDate,
                model.Currency,
                model.TaxableAmount,
                model.Taxes,
                model.TotalPrice,
                model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                new ImportIncomingInvoiceCommand.PartyInfo(
                    model.Customer.Id,
                    model.Customer.Name,
                    model.Customer.StreetName,
                    model.Customer.City,
                    model.Customer.PostalCode,
                    model.Customer.Country,
                    model.Customer.VatIndex,
                    model.Customer.NationalIdentificationNumber
                ),
                new ImportIncomingInvoiceCommand.PartyInfo(
                    model.Supplier.Id,
                    model.Supplier.Name,
                    model.Supplier.StreetName,
                    model.Supplier.City,
                    model.Supplier.PostalCode,
                    model.Supplier.Country,
                    model.Supplier.VatIndex,
                    model.Supplier.NationalIdentificationNumber
                ),
                model.LineItems.Select(i => new ImportIncomingInvoiceCommand.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription)).ToList(),
                model.PricesAreVatIncluded,
                model.PricesByVat.Select(p => new ImportIncomingInvoiceCommand.InvoicePriceByVat(p.TaxableAmount, p.VatRate, p.VatAmount, p.TotalPrice, p.ProvidenceFundAmount)).ToList(),
                model.NonTaxableItems.Select(i => new ImportIncomingInvoiceCommand.NonTaxableItem(i.Description, i.Amount)).ToList(),
                model.ProvidenceFundDescription,
                model.ProvidenceFundRate,
                model.ProvidenceFundAmount,
                model.WithholdingTaxDescription,
                model.WithholdingTaxRate,
                model.WithholdingTaxTaxableAmountRate,
                model.WithholdingTaxAmount
            );

            await Bus.Send(command);
        }

        public async Task LinkIncomingInvoiceToJobOrderAsync(LinkIncomingInvoiceToJobOrderModel model)
        {
            var command = new LinkIncomingInvoiceToJobOrderCommand(
                model.UserId,
                model.InvoiceId,
                model.JobOrderId,
                model.DateOfLink,
                model.Amount
            );

            await Bus.Send(command);
        }

        public async Task MarkIncomingInvoiceAsPaid(MarkIncomingInvoiceAsPaidModel model)
        {
            var command = new MarkIncomingInvoiceAsPaidCommand(
                model.UserId,
                model.InvoiceId,
                model.PaymentDate);

            await Bus.Send(command);
        }

        public async Task MarkIncomingInvoiceAsOverdue(MarkIncomingInvoiceAsOverdueModel model)
        {
            var command = new MarkIncomingInvoiceAsOverdueCommand(
                model.UserId,
                model.InvoiceId);

            await Bus.Send(command);
        }

        #endregion

        #region Outgoing invoices
        public async Task ImportOutgoingInvoiceAsync(ImportOutgoingInvoiceModel model)
        {
            var command = new ImportOutgoingInvoiceCommand(
                model.UserId,
                model.InvoiceId,
                model.InvoiceNumber,
                model.InvoiceDate,
                model.DueDate,
                model.Currency,
                model.TaxableAmount,
                model.Taxes,
                model.TotalPrice,
                model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                new ImportOutgoingInvoiceCommand.PartyInfo(
                    model.Customer.Id,
                    model.Customer.Name,
                    model.Customer.StreetName,
                    model.Customer.City,
                    model.Customer.PostalCode,
                    model.Customer.Country,
                    model.Customer.VatIndex,
                    model.Customer.NationalIdentificationNumber
                ),
                new ImportOutgoingInvoiceCommand.PartyInfo(
                    model.Supplier.Id,
                    model.Supplier.Name,
                    model.Supplier.StreetName,
                    model.Supplier.City,
                    model.Supplier.PostalCode,
                    model.Supplier.Country,
                    model.Supplier.VatIndex,
                    model.Supplier.NationalIdentificationNumber
                ),
                model.LineItems.Select(i => new ImportOutgoingInvoiceCommand.InvoiceLineItem(i.Code, i.Description, i.Quantity, i.UnitPrice, i.TotalPrice, i.Vat, i.VatDescription)).ToList(),
                model.PricesAreVatIncluded,
                model.PricesByVat.Select(p => new ImportOutgoingInvoiceCommand.InvoicePriceByVat(p.TaxableAmount, p.VatRate, p.VatAmount, p.TotalPrice, p.ProvidenceFundAmount)).ToList(),
                model.NonTaxableItems.Select(i => new ImportOutgoingInvoiceCommand.NonTaxableItem(i.Description, i.Amount)).ToList(),
                model.ProvidenceFundDescription,
                model.ProvidenceFundRate,
                model.ProvidenceFundAmount,
                model.WithholdingTaxDescription,
                model.WithholdingTaxRate,
                model.WithholdingTaxTaxableAmountRate,
                model.WithholdingTaxAmount
            );

            await Bus.Send(command);
        }

        public async Task LinkOutgoingInvoiceToJobOrderAsync(LinkOutgoingInvoiceToJobOrderModel model)
        {
            var command = new LinkOutgoingInvoiceToJobOrderCommand(
                model.UserId,
                model.InvoiceId,
                model.JobOrderId,
                model.DateOfLink,
                model.Amount
            );

            await Bus.Send(command);
        }

        public async Task MarkOutgoingInvoiceAsPaid(MarkOutgoingInvoiceAsPaidModel model)
        {
            var command = new MarkOutgoingInvoiceAsPaidCommand(
                model.UserId, 
                model.InvoiceId, 
                model.PaymentDate);

            await Bus.Send(command);
        }

        public async Task MarkOutgoingInvoiceAsOverdue(MarkOutgoingInvoiceAsOverdueModel model)
        {
            var command = new MarkOutgoingInvoiceAsOverdueCommand(
                model.UserId,
                model.InvoiceId);

            await Bus.Send(command);
        }
        #endregion
    }
}
