using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class IssueOutgoingInvoice
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        private IssueInvoiceForm.ViewModel model = new();

        private async Task IssueOutgoingInvoiceAsync(IssueInvoiceForm.ViewModel invoice)
        {
            var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(invoice.Customer!.OriginalId);

            var command = new IssueInvoiceCommand(
                AppContext.UserId,
                invoice.Date!.Value,
                invoice.Currency,
                invoice.Amount,
                invoice.Taxes,
                invoice.TotalPrice,
                invoice.TotalToPay,
                invoice.Description,
                invoice.PaymentTerms,
                invoice.PurchaseOrderNumber,
                invoice.Customer.OriginalId,
                invoice.Customer.Name,
                customerBillingInfo?.Address?.Address,
                customerBillingInfo?.Address?.City,
                customerBillingInfo?.Address?.PostalCode,
                customerBillingInfo?.Address?.Country,
                customerBillingInfo?.VatIndex,
                customerBillingInfo?.NationalIdentificationNumber,
                "", //supplier name
                "", //supplier address
                "", //supplier city
                "", //supplier postal code
                "", //supplier country
                "", //supplier VAT
                "", //supplier National identification number
                invoice.LineItems.Select(MapLineItemForIssue),
                invoice.VatIncluded,
                invoice.PricesByVat.Select(MapPriceByVatForIssue),
                invoice.NonTaxableItems.Select(MapNonTaxableItemForIssue),
                invoice.ProvidenceFund?.Description,
                invoice.ProvidenceFund?.Rate,
                invoice.ProvidenceFund?.Amount,
                invoice.WithholdingTax?.Description,
                invoice.WithholdingTax?.Rate,
                invoice.WithholdingTax?.TaxableAmountRate,
                invoice.WithholdingTax?.Amount);

            await Bus.Send(command);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeModel();
        }

        private void Cancel()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            model = new();
            model.LineItems.Add(new());
        }

        #region Mapping
        private IssueInvoiceCommand.InvoiceLineItem MapLineItemForIssue(InvoiceLineItem lineItem)
        {
            return new IssueInvoiceCommand.InvoiceLineItem(
                lineItem.Code,
                lineItem.Description,
                lineItem.Quantity,
                lineItem.TotalPrice,
                lineItem.UnitPrice,
                lineItem.Vat?.Rate ?? 0,
                lineItem.Vat?.Description);
        }

        private IssueInvoiceCommand.InvoicePriceByVat MapPriceByVatForIssue(InvoicePriceByVat priceByVat)
        {
            return new IssueInvoiceCommand.InvoicePriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private IssueInvoiceCommand.NonTaxableItem MapNonTaxableItemForIssue(NonTaxableItem nonTaxableItem)
            => new IssueInvoiceCommand.NonTaxableItem(nonTaxableItem.Description, nonTaxableItem.Amount);
        #endregion
    }
}
