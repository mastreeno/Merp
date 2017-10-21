using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using Rebus.Handlers;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class OutgoingInvoiceDenormalizer :
        IHandleMessages<OutgoingInvoiceIssuedEvent>,
        IHandleMessages<OutgoingInvoiceGotOverdueEvent>,
        IHandleMessages<OutgoingInvoicePaidEvent>,
        IHandleMessages<OutgoingInvoiceLinkedToJobOrderEvent>
    {
        private DbContextOptions<AccountancyDbContext> Options;

        public OutgoingInvoiceDenormalizer(DbContextOptions<AccountancyDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(OutgoingInvoiceIssuedEvent message)
        {
            var invoice = new OutgoingInvoice();
            invoice.TaxableAmount = message.TaxableAmount;
            invoice.Date = message.InvoiceDate;
            invoice.DueDate = message.DueDate;
            invoice.Description = message.Description;
            invoice.Number = message.InvoiceNumber;
            invoice.OriginalId = message.InvoiceId;
            invoice.PurchaseOrderNumber = message.PurchaseOrderNumber;
            invoice.Taxes = message.Taxes;
            invoice.TotalPrice = message.TotalPrice;
            invoice.IsOverdue = false;
            invoice.IsPaid = false;
            invoice.Customer = new Invoice.PartyInfo()
            {
                City = message.Customer.City,
                Country = message.Customer.Country,
                Name = message.Customer.Name,
                NationalIdentificationNumber = message.Customer.NationalIdentificationNumber,
                OriginalId = message.Customer.Id,
                PostalCode = message.Customer.PostalCode,
                StreetName = message.Customer.StreetName,
                VatIndex = message.Customer.VatIndex
            };
            invoice.Supplier = new Invoice.PartyInfo()
            {
                City = message.Supplier.City,
                Country = message.Supplier.Country,
                Name = message.Supplier.Name,
                NationalIdentificationNumber = message.Supplier.NationalIdentificationNumber,
                OriginalId = message.Supplier.Id,
                PostalCode = message.Supplier.PostalCode,
                StreetName = message.Supplier.StreetName,
                VatIndex = message.Supplier.VatIndex
            };
            using (var ctx = new AccountancyDbContext(Options))
            {
                ctx.OutgoingInvoices.Add(invoice);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(OutgoingInvoicePaidEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var invoice = ctx.OutgoingInvoices
                    .Where(i => i.OriginalId == message.InvoiceId)
                    .Single();
                invoice.IsPaid = true;
                invoice.IsOverdue = false;
                invoice.PaymentDate = message.PaymentDate;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(OutgoingInvoiceGotOverdueEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var invoice = ctx.OutgoingInvoices
                    .Where(i => i.OriginalId == message.InvoiceId)
                    .Single();
                invoice.IsOverdue = true;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(OutgoingInvoiceLinkedToJobOrderEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var invoice = ctx.OutgoingInvoices.Where(i => i.OriginalId == message.InvoiceId).Single();
                invoice.JobOrderId = message.JobOrderId;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
