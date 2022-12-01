using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterOutgoingCreditNote
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        private RegisterOutgoingInvoiceForm.ViewModel model = new();

        private async Task RegisterOutgoingCreditNoteAsync(RegisterOutgoingInvoiceForm.ViewModel creditNote)
        {
            //TODO
            var customerLegalInfo = await PartyApi.GetPartyLegalInfoByPartyIdAsync(creditNote.Customer!.OriginalId);

            var command = new RegisterOutgoingCreditNoteCommand(
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
                creditNote.Customer!.OriginalId,
                creditNote.Customer!.Name,
                customerLegalInfo?.Address?.Address,
                customerLegalInfo?.Address?.City,
                customerLegalInfo?.Address?.PostalCode,
                customerLegalInfo?.Address?.Country,
                customerLegalInfo?.VatIndex,
                customerLegalInfo?.NationalIdentificationNumber,
                "",
                "",
                "",
                "",
                "",
                "",
                "",
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
        private RegisterOutgoingCreditNoteCommand.LineItem MapLineItemForRegister(InvoiceLineItem lineItem)
        {
            return new RegisterOutgoingCreditNoteCommand.LineItem(
                lineItem.Code,
                lineItem.Description,
                lineItem.Quantity,
                lineItem.UnitPrice,
                lineItem.TotalPrice,
                lineItem.Vat!.Rate,
                lineItem.Vat!.Description);
        }

        private RegisterOutgoingCreditNoteCommand.PriceByVat MapPriceByVatForRegister(InvoicePriceByVat priceByVat)
        {
            return new RegisterOutgoingCreditNoteCommand.PriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private RegisterOutgoingCreditNoteCommand.NonTaxableItem MapNonTaxableItemForRegister(NonTaxableItem item)
            => new RegisterOutgoingCreditNoteCommand.NonTaxableItem(item.Description, item.Amount);
        #endregion
    }
}
