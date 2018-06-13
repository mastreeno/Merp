using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class IncomingInvoiceDenormalizer :
        IHandleMessages<IncomingInvoiceRegisteredEvent>,
        IHandleMessages<IncomingInvoiceGotOverdueEvent>,
        IHandleMessages<IncomingInvoicePaidEvent>,
        IHandleMessages<IncomingInvoiceLinkedToJobOrderEvent>
    {
        private DbContextOptions<AccountancyDbContext> Options;

        public IncomingInvoiceDenormalizer(DbContextOptions<AccountancyDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task Handle(IncomingInvoiceRegisteredEvent message)
        {
            var invoice = new IncomingInvoice();
            invoice.TaxableAmount = message.TaxableAmount;
            invoice.Currency = message.Currency;
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
                ctx.IncomingInvoices.Add(invoice);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(IncomingInvoicePaidEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var invoice = ctx.IncomingInvoices
                    .Where(i => i.OriginalId == message.InvoiceId)
                    .Single();
                invoice.IsPaid = true;
                invoice.IsOverdue = false;
                invoice.PaymentDate = message.PaymentDate;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(IncomingInvoiceGotOverdueEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var invoice = ctx.IncomingInvoices
                    .Where(i => i.OriginalId == message.InvoiceId)
                    .Single();
                invoice.IsOverdue = true;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(IncomingInvoiceLinkedToJobOrderEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var invoice = ctx.IncomingInvoices.Where(i => i.OriginalId == message.InvoiceId).Single();
                invoice.JobOrderId = message.JobOrderId;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
