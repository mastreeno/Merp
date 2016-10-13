using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using Rebus.Handlers;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class IncomingInvoiceDenormalizer :
        IHandleMessages<IncomingInvoiceRegisteredEvent>
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
    }
}
