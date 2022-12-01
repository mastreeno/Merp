using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterIncomingCreditNote
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        private RegisterIncomingInvoiceForm.ViewModel model = new();

        private async Task RegisterIncomingCreditNoteAsync(RegisterIncomingInvoiceForm.ViewModel creditNote)
        {
            //INCOMING -> Customer should be retrieved by settings
            var supplierLegalInfo = await PartyApi.GetPartyLegalInfoByPartyIdAsync(creditNote.Supplier!.OriginalId);

            var command = new RegisterIncomingCreditNoteCommand(
                AppContext.UserId,
                creditNote.Number,
                creditNote.Date!.Value,
                creditNote.Currency,
                creditNote.Amount,
                creditNote.Taxes,
                creditNote.TotalPrice,
                creditNote.TotalToPay,
                creditNote.Description,
                creditNote.PaymentTerms,
                creditNote.PurchaseOrderNumber,
                Guid.NewGuid(),
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                creditNote.Supplier!.OriginalId,
                creditNote.Supplier!.Name,
                supplierLegalInfo?.Address?.Address,
                supplierLegalInfo?.Address?.City,
                supplierLegalInfo?.Address?.PostalCode,
                supplierLegalInfo?.Address?.Country,
                supplierLegalInfo?.VatIndex,
                supplierLegalInfo?.NationalIdentificationNumber,
                creditNote.LineItems.Select(MapLineItemForRegister),
                creditNote.VatIncluded,
                creditNote.PricesByVat.Select(MapPriceByVatForRegister),
                creditNote.NonTaxableItems.Select(MapNonTaxableItemForRegister),
                creditNote.ProvidenceFund?.Description,
                creditNote.ProvidenceFund?.Rate,
                creditNote.ProvidenceFund?.Amount,
                creditNote.WithholdingTax?.Description,
                creditNote.WithholdingTax?.Rate,
                creditNote.WithholdingTax?.TaxableAmountRate,
                creditNote.WithholdingTax?.Amount);

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
        private RegisterIncomingCreditNoteCommand.LineItem MapLineItemForRegister(InvoiceLineItem lineItem)
        {
            return new RegisterIncomingCreditNoteCommand.LineItem(
                lineItem.Code,
                lineItem.Description,
                lineItem.Quantity,
                lineItem.TotalPrice,
                lineItem.UnitPrice,
                lineItem.Vat?.Rate ?? 0,
                lineItem.Vat?.Description);
        }

        private RegisterIncomingCreditNoteCommand.PriceByVat MapPriceByVatForRegister(InvoicePriceByVat priceByVat)
        {
            return new RegisterIncomingCreditNoteCommand.PriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private RegisterIncomingCreditNoteCommand.NonTaxableItem MapNonTaxableItemForRegister(NonTaxableItem nonTaxableItem)
            => new RegisterIncomingCreditNoteCommand.NonTaxableItem(nonTaxableItem.Description, nonTaxableItem.Amount);
        #endregion
    }
}
