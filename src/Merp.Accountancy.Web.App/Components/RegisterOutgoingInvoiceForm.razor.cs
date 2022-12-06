using Merp.Accountancy.Web.App.Model;
using Merp.Accountancy.Web.App.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class RegisterOutgoingInvoiceForm
    {
        [Inject]
        public IAccountancySettingsProvider AccountancySettings { get; set; } = default!;

        [Parameter]
        public ViewModel Model { get; set; } = new();

        [Parameter]
        public EventCallback<ViewModel> OnRegisterOutgoingInvoice { get; set; }

        [Parameter]
        public bool HideDueDate { get; set; } = false;

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private async Task SubmitAsync() => await OnRegisterOutgoingInvoice.InvokeAsync(Model);

        private async Task CancelAsync() => await OnCancel.InvokeAsync();

        private IEnumerable<Vat> vatList = Array.Empty<Vat>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            vatList = AccountancySettings.GetVatList();

            foreach (var item in Model.LineItems)
            {
                if (item.Vat is null)
                {
                    item.Vat = vatList.FirstOrDefault();
                }
            }
        }

        private void OnVatIncludedChange(bool value)
        {
            Model.VatIncluded = value;
            CalculatePricesByVat();
        }

        #region Invoice totals
        private void CalculateInvoiceTotals()
        {
            Model.TotalPrice = CalculateInvoiceTotalPrice(Model.PricesByVat, Model.NonTaxableItems);
            Model.Taxes = CalculateInvoiceTotalTaxes(Model.PricesByVat);
            Model.Amount = CalculateInvoiceTaxableAmount(Model.PricesByVat);

            if (Model.ProvidenceFund is not null)
            {
                Model.ProvidenceFund.Amount = CalculateTotalProvidenceFundAmount(Model.PricesByVat);
            }

            if (Model.WithholdingTax is not null)
            {
                Model.WithholdingTax.Amount = CalculateWithholdingTaxAmount(Model.Amount);
            }

            Model.TotalToPay = CalculateInvoiceTotalToPay();
        }

        private decimal CalculateInvoiceTotalPrice(List<InvoicePriceByVat> pricesByVat, List<NonTaxableItem> nonTaxableItems)
        {
            decimal vatTotalPrice = 0;
            if (pricesByVat.Any())
            {
                vatTotalPrice = pricesByVat.Select(p => p.TotalPrice).Sum();
            }

            decimal nonTaxablePrice = 0;
            if (nonTaxableItems.Any())
            {
                nonTaxablePrice = nonTaxableItems.Select(t => t.Amount).Sum();
            }

            return vatTotalPrice + nonTaxablePrice;
        }

        private decimal CalculateInvoiceTotalTaxes(List<InvoicePriceByVat> pricesByVat)
        {
            if (!pricesByVat.Any())
            {
                return 0;
            }

            return pricesByVat.Select(p => p.VatAmount).Sum();
        }

        private decimal CalculateInvoiceTaxableAmount(List<InvoicePriceByVat> pricesByVat)
        {
            if (!pricesByVat.Any())
            {
                return 0;
            }

            return pricesByVat.Select(p => p.TaxableAmount).Sum();
        }

        private decimal CalculateTotalProvidenceFundAmount(List<InvoicePriceByVat> pricesByVat)
        {
            if (!pricesByVat.Any())
            {
                return 0;
            }

            return pricesByVat.Select(p => p.ProvidenceFundAmount).Sum();
        }

        private decimal CalculateWithholdingTaxAmount(decimal taxableAmount)
        {
            if (Model.ProvidenceFund is not null && !Model.ProvidenceFund.AppliedInWithholdingTax)
            {
                taxableAmount -= Model.ProvidenceFund.Amount;
            }

            var withholdingTaxableAmount = taxableAmount * (Model.WithholdingTax!.TaxableAmountRate / 100);
            var withholdingTaxAmount = withholdingTaxableAmount * (Model.WithholdingTax!.Rate / 100);

            return withholdingTaxAmount;
        }

        private decimal CalculateInvoiceTotalToPay()
        {
            var withholdingTaxAmount = Model.WithholdingTax?.Amount ?? 0;
            return Model.TotalPrice - withholdingTaxAmount;
        }
        #endregion

        #region Prices by VAT
        private void CalculatePricesByVat()
        {
            var uniqueVatRates = Model.LineItems.Where(l => l.Vat != null).Select(l => l.Vat!).Distinct();

            var pricesByVat = new List<InvoicePriceByVat>();
            if (uniqueVatRates.Any())
            {
                foreach (var vat in uniqueVatRates)
                {
                    var lineItemsWithVat = Model.LineItems.Where(l => l.Vat == vat);

                    var taxableAmount = CalculateTaxableAmountForVat(lineItemsWithVat);
                    var providenceFundAmount = CalculateProvidenceFundAmountForVat(lineItemsWithVat);
                    var vatAmount = CalculateVatAmountForVat(lineItemsWithVat);
                    var totalPrice = CalculateTotalPriceForVat(lineItemsWithVat);

                    pricesByVat.Add(new InvoicePriceByVat
                    {
                        TaxableAmount = taxableAmount,
                        ProvidenceFundAmount = providenceFundAmount,
                        VatAmount = vatAmount,
                        VatRate = vat.Rate,
                        TotalPrice = totalPrice,
                    });
                }
            }

            Model.PricesByVat = pricesByVat;
            CalculateInvoiceTotals();
        }

        private decimal CalculateTotalPriceForVat(IEnumerable<InvoiceLineItem> lineItems)
        {
            return lineItems.Select(i =>
            {
                if (Model.VatIncluded)
                {
                    return i.TotalPrice;
                }

                var providenceFundAmount = CalculateProvidenceFundForAmount(i.TotalPrice);
                var taxableAmount = i.TotalPrice + providenceFundAmount;
                var vatAmount = taxableAmount * (i.Vat!.Rate / 100);

                return taxableAmount + vatAmount;
            }).Sum();
        }

        private decimal CalculateVatAmountForVat(IEnumerable<InvoiceLineItem> lineItems)
        {
            return lineItems.Select(i =>
            {
                var vatRate = i.Vat!.Rate / 100;
                if (Model.VatIncluded)
                {
                    var total = i.TotalPrice / (1 + vatRate);
                    return total * vatRate;
                }

                var providenceFundAmount = CalculateProvidenceFundForAmount(i.TotalPrice);
                return (i.TotalPrice + providenceFundAmount) * vatRate;
            }).Sum();
        }

        private decimal CalculateProvidenceFundAmountForVat(IEnumerable<InvoiceLineItem> lineItems)
        {
            return lineItems.Select(i =>
            {
                var vatRate = i.Vat!.Rate / 100;
                var amount = i.TotalPrice;

                if (Model.VatIncluded)
                {
                    amount = i.TotalPrice / (1 + vatRate);
                }

                return CalculateProvidenceFundForAmount(amount);
            }).Sum();
        }

        private decimal CalculateTaxableAmountForVat(IEnumerable<InvoiceLineItem> lineItems)
        {
            return lineItems.Select(i =>
            {
                var vatRate = i.Vat!.Rate / 100;

                if (Model.VatIncluded)
                {
                    return i.TotalPrice / (1 + vatRate);
                }

                var providenceFundAmount = CalculateProvidenceFundForAmount(i.TotalPrice);
                return i.TotalPrice + providenceFundAmount;
            }).Sum();
        }
        #endregion

        private decimal CalculateProvidenceFundForAmount(decimal amount)
        {
            if (Model.ProvidenceFund is null)
            {
                return 0;
            }

            return amount * (Model.ProvidenceFund.Rate / 100);
        }

        public class ViewModel
        {
            [Required]
            public PartyInfoAutocomplete.ViewModel? Customer { get; set; }

            [Required]
            public string Number { get; set; } = string.Empty;

            [Required]
            public DateTime? Date { get; set; } = DateTime.Today;

            public DateTime? DueDate { get; set; }

            [Required]
            public string Currency { get; set; } = string.Empty;

            [Required]
            public decimal Amount { get; set; }

            [Required]
            public decimal Taxes { get; set; }

            [Required]
            public decimal TotalPrice { get; set; }

            public decimal TotalToPay { get; set; }

            [Required]
            public string Description { get; set; } = string.Empty;

            public string? PaymentTerms { get; set; }

            public string? PurchaseOrderNumber { get; set; }

            public bool VatIncluded { get; set; }

            public List<InvoiceLineItem> LineItems { get; set; } = new();

            public List<InvoicePriceByVat> PricesByVat { get; set; } = new();

            public List<NonTaxableItem> NonTaxableItems { get; set; } = new();

            public ProvidenceFund? ProvidenceFund { get; set; }

            public WithholdingTax? WithholdingTax { get; set; }
        }
    }
}
