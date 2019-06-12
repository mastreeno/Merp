using Merp.Accountancy.Drafts.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.Drafts.Commands
{
    public class OutgoingInvoiceCommands : IDisposable
    {
        private readonly AccountancyDraftsDbContext _context;

        public OutgoingInvoiceCommands(AccountancyDraftsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public async Task CreateDraft(PartyInfo customer, DateTime? date, string currency, decimal taxableAmount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, bool pricesAreVatIncluded, IEnumerable<DraftLineItem> lineItems, IEnumerable<PriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems, string providenceFundDescription, decimal? providenceFundRate, decimal? providenceFundAmount, string withholdingTaxDescription, decimal? withholdingTaxRate, decimal? withholdingTaxTaxableAmountRate, decimal? withholdingTaxAmount, decimal totalToPay)
        {
            var draft = new OutgoingInvoiceDraft
            {
                Id = Guid.NewGuid(),
                Currency = currency,
                Customer = customer,
                Date = date,
                Description = description,
                LineItems = lineItems.ToList(),
                NonTaxableItems = nonTaxableItems.ToList(),
                PricesAreVatIncluded = pricesAreVatIncluded,
                PricesByVat = pricesByVat.ToList(),
                PurchaseOrderNumber = purchaseOrderNumber,
                PaymentTerms = paymentTerms,
                TaxableAmount = taxableAmount,
                Taxes = taxes,
                TotalPrice = totalPrice,
                TotalToPay = totalToPay
            };

            if (!string.IsNullOrWhiteSpace(providenceFundDescription) && providenceFundRate.HasValue && providenceFundAmount.HasValue)
            {
                draft.ProvidenceFund = new ProvidenceFund
                {
                    Amount = providenceFundAmount,
                    Description = providenceFundDescription,
                    Rate = providenceFundRate
                };
            }

            if (!string.IsNullOrWhiteSpace(withholdingTaxDescription) && withholdingTaxRate.HasValue && withholdingTaxTaxableAmountRate.HasValue && withholdingTaxAmount.HasValue)
            {
                draft.WithholdingTax = new WithholdingTax
                {
                    Amount = withholdingTaxAmount.Value,
                    Description = withholdingTaxDescription,
                    Rate = withholdingTaxRate.Value,
                    TaxableAmountRate = withholdingTaxTaxableAmountRate.Value
                };
            }

            _context.Add(draft);
            await _context.SaveChangesAsync();
        }
        
        public async Task EditDraft(Guid draftId, PartyInfo customer, DateTime? date, string currency, decimal taxableAmount, decimal taxes, decimal totalPrice, string description, string paymentTerms, string purchaseOrderNumber, bool pricesAreVatIncluded, IEnumerable<DraftLineItem> lineItems, IEnumerable<PriceByVat> pricesByVat, IEnumerable<NonTaxableItem> nonTaxableItems, string providenceFundDescription, decimal? providenceFundRate, decimal? providenceFundAmount, string withholdingTaxDescription, decimal? withholdingTaxRate, decimal? withholdingTaxTaxableAmountRate, decimal? withholdingTaxAmount, decimal totalToPay)
        {
            var draft = _context
                .OutgoingInvoiceDrafts
                .Include(d => d.LineItems)
                .Include(d => d.PricesByVat)
                .Include(d => d.NonTaxableItems)
                .Single(d => d.Id == draftId);

            draft.Customer = customer;

            if (draft.Date != date)
            {
                draft.Date = date;
            }
            if (draft.Currency != currency)
            {
                draft.Currency = currency;
            }
            if (draft.TaxableAmount != taxableAmount)
            {
                draft.TaxableAmount = taxableAmount;
            }
            if (draft.Taxes != taxes)
            {
                draft.Taxes = taxes;
            }
            if (draft.TotalPrice != totalPrice)
            {
                draft.TotalPrice = totalPrice;
            }
            if (draft.TotalToPay != totalToPay)
            {
                draft.TotalToPay = totalToPay;
            }
            if (draft.Description != description)
            {
                draft.Description = description;
            }
            if (draft.PaymentTerms != paymentTerms)
            {
                draft.PaymentTerms = paymentTerms;
            }
            if (draft.PurchaseOrderNumber != purchaseOrderNumber)
            {
                draft.PurchaseOrderNumber = purchaseOrderNumber;
            }
            if (draft.PricesAreVatIncluded != pricesAreVatIncluded)
            {
                draft.PricesAreVatIncluded = pricesAreVatIncluded;
            }
            if (draft.ProvidenceFund.Description != providenceFundDescription || draft.ProvidenceFund.Rate != providenceFundRate || draft.ProvidenceFund.Amount != providenceFundAmount)
            {
                draft.ProvidenceFund = new ProvidenceFund
                {
                    Description = providenceFundDescription,
                    Amount = providenceFundAmount,
                    Rate = providenceFundRate
                };
            }
            if (draft.WithholdingTax.Description != withholdingTaxDescription || draft.WithholdingTax.Rate != withholdingTaxRate || draft.WithholdingTax.TaxableAmountRate != withholdingTaxTaxableAmountRate || draft.WithholdingTax.Amount != withholdingTaxAmount)
            {
                draft.WithholdingTax = new WithholdingTax
                {
                    Amount = withholdingTaxAmount.Value,
                    Description = withholdingTaxDescription,
                    Rate = withholdingTaxRate.Value,
                    TaxableAmountRate = withholdingTaxTaxableAmountRate.Value
                };
            }

            UpdateDraftLineItems(draft, lineItems);
            UpdateDraftPricesByVat(draft, pricesByVat);
            UpdateDraftNonTaxableItems(draft, nonTaxableItems);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDraft(Guid draftId)
        {
            var draft = _context
                .OutgoingInvoiceDrafts
                .Include(d => d.LineItems)
                .Include(d => d.PricesByVat)
                .Include(d => d.NonTaxableItems)
                .Single(d => d.Id == draftId);

            _context.Remove(draft);

            await _context.SaveChangesAsync();
        }

        #region Private Methods
        private void UpdateDraftLineItems(OutgoingInvoiceDraft draft, IEnumerable<DraftLineItem> lineItems)
        {
            var draftLineItemIds = draft.LineItems.Select(li => li.Id);

            var lineItemsToAdd = lineItems.Where(l => !draftLineItemIds.Contains(l.Id));
            var lineItemsToUpdate = lineItems.Where(l => draftLineItemIds.Contains(l.Id));
            var lineItemsToRemove = draft.LineItems.Where(li => !lineItems.Any(l => l.Id == li.Id));

            if (lineItemsToRemove.Any())
            {
                lineItemsToRemove
                    .ToList()
                    .ForEach(l => draft.LineItems.Remove(l));
            }

            if (lineItemsToUpdate.Any())
            {
                lineItemsToUpdate
                    .ToList()
                    .ForEach(l =>
                    {
                        var lineItem = draft.LineItems.Single(li => li.Id == l.Id);
                        if (lineItem.Code != l.Code)
                        {
                            lineItem.Code = l.Code;
                        }
                        if (lineItem.Description != l.Description)
                        {
                            lineItem.Description = l.Description;
                        }
                        if (lineItem.Quantity != l.Quantity)
                        {
                            lineItem.Quantity = l.Quantity;
                        }
                        if (lineItem.TotalPrice != l.TotalPrice)
                        {
                            lineItem.TotalPrice = l.TotalPrice;
                        }
                        if (lineItem.UnitPrice != l.UnitPrice)
                        {
                            lineItem.UnitPrice = l.UnitPrice;
                        }
                        if (lineItem.Vat != l.Vat)
                        {
                            lineItem.Vat = l.Vat;
                        }
                        if (lineItem.VatDescription != l.VatDescription)
                        {
                            lineItem.VatDescription = l.VatDescription;
                        }
                    });
            }

            if (lineItemsToAdd.Any())
            {
                draft.LineItems.AddRange(lineItemsToAdd);
            }
        }

        private void UpdateDraftPricesByVat(OutgoingInvoiceDraft draft, IEnumerable<PriceByVat> pricesByVat)
        {
            draft.PricesByVat.Clear();
            draft.PricesByVat.AddRange(pricesByVat);
        }

        private void UpdateDraftNonTaxableItems(OutgoingInvoiceDraft draft, IEnumerable<NonTaxableItem> nonTaxableItems)
        {
            var nonTaxableItemIds = draft.NonTaxableItems.Select(t => t.Id);

            var nonTaxableItemsToAdd = nonTaxableItems.Where(nt => !nonTaxableItemIds.Contains(nt.Id));
            var nonTaxableItemsToUpdate = nonTaxableItems.Where(nt => nonTaxableItemIds.Contains(nt.Id));
            var nonTaxableItemsToRemove = draft.NonTaxableItems.Where(nt => !nonTaxableItems.Any(t => t.Id == nt.Id));

            if (nonTaxableItemsToRemove.Any())
            {
                nonTaxableItemsToRemove
                    .ToList()
                    .ForEach(nt => draft.NonTaxableItems.Remove(nt));
            }

            if (nonTaxableItemsToUpdate.Any())
            {
                nonTaxableItemsToUpdate
                    .ToList()
                    .ForEach(nt =>
                    {
                        var nonTaxableItem = draft.NonTaxableItems.Single(t => t.Id == nt.Id);
                        if (nonTaxableItem.Amount != nt.Amount)
                        {
                            nonTaxableItem.Amount = nt.Amount;
                        }
                        if (nonTaxableItem.Description != nt.Description)
                        {
                            nonTaxableItem.Description = nt.Description;
                        }
                    });
            }

            if (nonTaxableItemsToAdd.Any())
            {
                draft.NonTaxableItems.AddRange(nonTaxableItemsToAdd);
            }
        }
        #endregion
    }
}
