using Merp.Accountancy.CommandStack.Events;
using Merp.Accountancy.QueryStack.Model;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.QueryStack.Denormalizers
{
    public class OutgoingCreditNoteDenormalizer :
        IHandleMessages<OutgoingCreditNoteIssuedEvent>,
        IHandleMessages<OutgoingCreditNoteLinkedToJobOrderEvent>
    {
        private DbContextOptions<AccountancyDbContext> Options;

        public OutgoingCreditNoteDenormalizer(DbContextOptions<AccountancyDbContext> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(OutgoingCreditNoteIssuedEvent message)
        {
            var creditNote = new OutgoingCreditNote();
            creditNote.TaxableAmount = message.TaxableAmount;
            creditNote.Currency = message.Currency;
            creditNote.Date = message.CreditNoteDate;
            creditNote.Description = message.Description;
            creditNote.Number = message.CreditNoteNumber;
            creditNote.OriginalId = message.CreditNoteId;
            creditNote.PurchaseOrderNumber = message.PurchaseOrderNumber;
            creditNote.Taxes = message.Taxes;
            creditNote.TotalPrice = message.TotalPrice;
            creditNote.TotalToPay = message.TotalToPay;
            creditNote.IsOverdue = false;
            creditNote.IsPaid = false;
            creditNote.Customer = new Invoice.PartyInfo()
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
            creditNote.Supplier = new Invoice.PartyInfo()
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

            if (message.LineItems != null && message.LineItems.Count() > 0)
            {
                creditNote.InvoiceLineItems = message.LineItems.Select(i => new InvoiceLineItem
                {
                    Code = i.Code,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    Vat = i.Vat,
                    VatDescription = i.VatDescription
                }).ToList();
            }

            if (message.PricesByVat != null && message.PricesByVat.Count() > 0)
            {
                creditNote.PricesByVat = message.PricesByVat.Select(p => new PriceByVat
                {
                    TaxableAmount = p.TaxableAmount,
                    TotalPrice = p.TotalPrice,
                    VatAmount = p.VatAmount,
                    VatRate = p.VatRate
                }).ToList();
            }

            if (message.NonTaxableItems != null && message.NonTaxableItems.Count() > 0)
            {
                creditNote.NonTaxableItems = message.NonTaxableItems.Select(t => new NonTaxableItem
                {
                    Description = t.Description,
                    Amount = t.Amount
                }).ToList();
            }

            creditNote.PricesAreVatIncluded = message.PricesAreVatIncluded;

            if (!string.IsNullOrWhiteSpace(message.ProvidenceFundDescription) && message.ProvidenceFundRate.HasValue && message.ProvidenceFundAmount.HasValue)
            {
                creditNote.ProvidenceFund = new ProvidenceFund
                {
                    Amount = message.ProvidenceFundAmount.Value,
                    Description = message.ProvidenceFundDescription,
                    Rate = message.ProvidenceFundRate.Value
                };
            }

            if (!string.IsNullOrWhiteSpace(message.WithholdingTaxDescription) && message.WithholdingTaxRate.HasValue && message.WithholdingTaxTaxableAmountRate.HasValue && message.WithholdingTaxAmount.HasValue)
            {
                creditNote.WithholdingTax = new WithholdingTax
                {
                    Amount = message.WithholdingTaxAmount.Value,
                    Description = message.WithholdingTaxDescription,
                    Rate = message.WithholdingTaxRate.Value,
                    TaxableAmountRate = message.WithholdingTaxTaxableAmountRate.Value
                };
            }

            using (var ctx = new AccountancyDbContext(Options))
            {
                ctx.OutgoingCreditNotes.Add(creditNote);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Handle(OutgoingCreditNoteLinkedToJobOrderEvent message)
        {
            using (var ctx = new AccountancyDbContext(Options))
            {
                var creditNote = ctx.OutgoingCreditNotes.Where(i => i.OriginalId == message.CreditNoteId).Single();
                creditNote.JobOrderId = message.JobOrderId;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
