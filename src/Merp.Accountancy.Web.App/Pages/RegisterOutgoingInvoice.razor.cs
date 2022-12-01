﻿using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterOutgoingInvoice
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        private RegisterOutgoingInvoiceForm.ViewModel model = new();

        private async Task RegisterOutgoingInvoiceAsync(RegisterOutgoingInvoiceForm.ViewModel invoice)
        {
            var customerLegalInfo = await PartyApi.GetPartyLegalInfoByPartyIdAsync(invoice.Customer!.OriginalId);

            var command = new RegisterOutgoingInvoiceCommand(
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
                invoice.Customer!.OriginalId,
                invoice.Customer!.Name,
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
        private RegisterOutgoingInvoiceCommand.NonTaxableItem MapNonTaxableItemForRegister(NonTaxableItem item)
            => new RegisterOutgoingInvoiceCommand.NonTaxableItem(item.Description, item.Amount);

        private RegisterOutgoingInvoiceCommand.InvoicePriceByVat MapPriceByVatForRegister(InvoicePriceByVat priceByVat)
        {
            return new RegisterOutgoingInvoiceCommand.InvoicePriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private RegisterOutgoingInvoiceCommand.InvoiceLineItem MapLineItemForRegister(InvoiceLineItem lineItem)
        {
            return new RegisterOutgoingInvoiceCommand.InvoiceLineItem(
                lineItem.Code,
                lineItem.Description,
                lineItem.Quantity,
                lineItem.UnitPrice,
                lineItem.TotalPrice,
                lineItem.Vat!.Rate,
                lineItem.Vat!.Description);
        }
        #endregion
    }
}
