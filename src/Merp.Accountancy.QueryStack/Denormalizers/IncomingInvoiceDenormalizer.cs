using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using Rebus.Handlers;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class IncomingInvoiceDenormalizer :
        IHandleMessages<IncomingInvoiceRegisteredEvent>,
        IHandleMessages<IncomingInvoiceExpiredEvent>,
        IHandleMessages<IncomingInvoicePaidEvent>
    {
        public async Task Handle(IncomingInvoiceRegisteredEvent message)
        {
            var invoice = new IncomingInvoice();
            invoice.Amount = message.Amount;
            invoice.Date = message.InvoiceDate;
            invoice.Description = message.Description;
            invoice.Number = message.InvoiceNumber;
            invoice.OriginalId = message.InvoiceId;
            invoice.PurchaseOrderNumber = message.PurchaseOrderNumber;
            invoice.Taxes = message.Taxes;
            invoice.TotalPrice = message.TotalPrice;
            invoice.IsExpired = false;
            invoice.IsPaid = false;
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
            using (var ctx = new AccountancyContext())
            {
                ctx.IncomingInvoices.Add(invoice);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(IncomingInvoicePaidEvent message)
        {
            using (var ctx = new AccountancyContext())
            {
                var invoice = ctx.IncomingInvoices
                    .Where(i => i.OriginalId == message.InvoiceId)
                    .Single();
                invoice.IsPaid = true;
                invoice.PaymentDate = message.PaymentDate;
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(IncomingInvoiceExpiredEvent message)
        {
            using (var ctx = new AccountancyContext())
            {
                var invoice = ctx.IncomingInvoices
                    .Where(i => i.OriginalId == message.InvoiceId)
                    .Single();
                invoice.IsExpired = true;
                invoice.DueDate = message.DueDate;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
