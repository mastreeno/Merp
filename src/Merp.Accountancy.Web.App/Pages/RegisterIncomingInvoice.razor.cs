using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterIncomingInvoice
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        private RegisterIncomingInvoiceForm.ViewModel model = new();

        private async Task RegisterIncomingInvoiceAsync(RegisterIncomingInvoiceForm.ViewModel invoice)
        {
            //INCOMING -> Customer should be retrieved by settings

            var supplierBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(invoice.Supplier!.OriginalId);

            var command = new RegisterIncomingInvoiceCommand(
                AppContext.UserId,
                invoice.Number,
                invoice.Date!.Value,
                invoice.DueDate,
                invoice.Currency,
                invoice.Amount,
                invoice.Taxes,
                invoice.TotalPrice,
                invoice.TotalToPay,
                invoice.Description,
                invoice.PaymentTerms,
                invoice.PurchaseOrderNumber,
                Guid.NewGuid(),
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                invoice.Supplier!.OriginalId,
                invoice.Supplier!.Name,
                supplierBillingInfo?.Address?.Address,
                supplierBillingInfo?.Address?.City,
                supplierBillingInfo?.Address?.PostalCode,
                supplierBillingInfo?.Address?.Country,
                supplierBillingInfo?.VatIndex,
                supplierBillingInfo?.NationalIdentificationNumber,
                invoice.LineItems.Select(MapLineItemForRegister),
                invoice.VatIncluded,
                invoice.PricesByVat.Select(MapPriceByVatForRegister),
                invoice.NonTaxableItems.Select(MapNonTaxableItemForRegister),
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
        private RegisterIncomingInvoiceCommand.InvoiceLineItem MapLineItemForRegister(InvoiceLineItem lineItem)
        {
            return new RegisterIncomingInvoiceCommand.InvoiceLineItem(
                lineItem.Code,
                lineItem.Description,
                lineItem.Quantity,
                lineItem.TotalPrice,
                lineItem.UnitPrice,
                lineItem.Vat?.Rate ?? 0,
                lineItem.Vat?.Description);
        }

        private RegisterIncomingInvoiceCommand.InvoicePriceByVat MapPriceByVatForRegister(InvoicePriceByVat priceByVat)
        {
            return new RegisterIncomingInvoiceCommand.InvoicePriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private RegisterIncomingInvoiceCommand.NonTaxableItem MapNonTaxableItemForRegister(NonTaxableItem nonTaxableItem)
            => new RegisterIncomingInvoiceCommand.NonTaxableItem(nonTaxableItem.Description, nonTaxableItem.Amount);
        #endregion
    }
}
