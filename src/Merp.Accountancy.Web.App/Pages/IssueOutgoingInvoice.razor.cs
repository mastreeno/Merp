using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Drafts.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Accountancy.Web.App.Services;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class IssueOutgoingInvoice
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        [Inject] public IAccountancySettingsProvider AccountancySettings { get; set; } = default!;

        [Inject] public OutgoingInvoiceCommands DraftCommands { get; set; } = default!;

        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private IssueInvoiceForm.ViewModel model = new();

        private async Task IssueOutgoingInvoiceAsync(IssueInvoiceForm.ViewModel invoice)
        {
            var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(invoice.Customer!.OriginalId);
            var invoicingSettings = AccountancySettings.GetInvoicingSettings();

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
                invoicingSettings.CompanyName,
                invoicingSettings.Address,
                invoicingSettings.City,
                invoicingSettings.PostalCode,
                invoicingSettings.Country,
                invoicingSettings.TaxId,
                invoicingSettings.NationalIdentificationNumber,
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

        private async Task SaveOutgoingInvoiceDraftAsync(IssueInvoiceForm.ViewModel invoice)
        {
            try
            {
                var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(invoice.Customer!.OriginalId);
                var customer = new Drafts.Model.PartyInfo
                {
                    OriginalId = invoice.Customer!.OriginalId,
                    Name = invoice.Customer!.Name,
                    City = customerBillingInfo?.Address?.City,
                    Country = customerBillingInfo?.Address?.Country,
                    NationalIdentificationNumber = customerBillingInfo?.NationalIdentificationNumber,
                    PostalCode = customerBillingInfo?.Address?.PostalCode,
                    StreetName = customerBillingInfo?.Address?.Address,
                    VatIndex = customerBillingInfo?.VatIndex
                };

                await DraftCommands.CreateDraft(
                    customer,
                    invoice.Date,
                    invoice.Currency,
                    invoice.Amount,
                    invoice.Taxes,
                    invoice.TotalPrice,
                    invoice.Description,
                    invoice.PaymentTerms,
                    invoice.PurchaseOrderNumber,
                    invoice.VatIncluded,
                    invoice.LineItems.Select(MapLineItemForDraft),
                    invoice.PricesByVat.Select(MapPriceByVatForDraft),
                    invoice.NonTaxableItems.Select(MapNonTaxableItemForDraft),
                    invoice.ProvidenceFund?.Description,
                    invoice.ProvidenceFund?.Rate,
                    invoice.ProvidenceFund?.Amount,
                    invoice.WithholdingTax?.Description,
                    invoice.WithholdingTax?.Rate,
                    invoice.WithholdingTax?.TaxableAmountRate,
                    invoice.WithholdingTax?.Amount,
                    invoice.TotalToPay);

                Snackbar.Add(localizer[nameof(Resources.Pages.IssueOutgoingInvoice.DraftSaveSuccessMessage)], Severity.Success);
                NavigationManager.NavigateTo(UrlBuilder.BuildSearchDraftsUrl());
            }
            catch
            {
                Snackbar.Add(localizer[nameof(Resources.Pages.IssueOutgoingInvoice.DraftSaveErrorMessage)], Severity.Error);
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;

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
        private Drafts.Model.DraftLineItem MapLineItemForDraft(InvoiceLineItem lineItem)
        {
            return new Drafts.Model.DraftLineItem
            {
                Code = lineItem.Code,
                Description = lineItem.Description,
                Quantity = lineItem.Quantity,
                TotalPrice = lineItem.TotalPrice,
                UnitPrice = lineItem.UnitPrice,
                Vat = lineItem.Vat?.Rate ?? 0,
                VatDescription = lineItem.Vat?.Description
            };
        }

        private Drafts.Model.PriceByVat MapPriceByVatForDraft(InvoicePriceByVat priceByVat)
        {
            return new Drafts.Model.PriceByVat
            {
                ProvidenceFundAmount = priceByVat.ProvidenceFundAmount,
                TaxableAmount = priceByVat.TaxableAmount,
                TotalPrice = priceByVat.TotalPrice,
                VatAmount = priceByVat.VatAmount,
                VatRate = priceByVat.VatRate
            };
        }

        private Drafts.Model.NonTaxableItem MapNonTaxableItemForDraft(NonTaxableItem nonTaxableItem)
        {
            return new Drafts.Model.NonTaxableItem
            {
                Amount = nonTaxableItem.Amount,
                Description = nonTaxableItem.Description
            };
        }

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
