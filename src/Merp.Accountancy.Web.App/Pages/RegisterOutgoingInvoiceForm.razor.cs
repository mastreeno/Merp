using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterOutgoingInvoiceForm
    {
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
