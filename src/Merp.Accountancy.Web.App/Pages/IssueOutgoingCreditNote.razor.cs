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
    public partial class IssueOutgoingCreditNote
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        [Inject] public IAccountancySettingsProvider AccountancySettings { get; set; } = default!;

        [Inject] public OutgoingInvoiceCommands DraftCommands { get; set; } = default!;

        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private IssueInvoiceForm.ViewModel model = new();

        private async Task IssueOutgoingCreditNoteAsync(IssueInvoiceForm.ViewModel creditNote)
        {
            var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(creditNote.Customer!.OriginalId);
            var invoicingSettings = AccountancySettings.GetInvoicingSettings();

            var command = new IssueCreditNoteCommand(
                AppContext.UserId,
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
                creditNote.LineItems.Select(MapLineItemForIssue),
                creditNote.VatIncluded,
                creditNote.PricesByVat.Select(MapPriceByVatForIssue),
                creditNote.NonTaxableItems.Select(MapNonTaxableItemForIssue),
                creditNote.ProvidenceFund?.Description,
                creditNote.ProvidenceFund?.Rate,
                creditNote.ProvidenceFund?.Amount,
                creditNote.WithholdingTax?.Description,
                creditNote.WithholdingTax?.Rate,
                creditNote.WithholdingTax?.TaxableAmountRate,
                creditNote.WithholdingTax?.Amount);

            await Bus.Send(command);
        }

        private async Task SaveOutgoingCreditNoteDraftAsync(IssueInvoiceForm.ViewModel creditNote)
        {
            try
            {
                var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(creditNote.Customer!.OriginalId);
                var customer = new Drafts.Model.PartyInfo
                {
                    OriginalId = creditNote.Customer!.OriginalId,
                    Name = creditNote.Customer!.Name,
                    City = customerBillingInfo?.Address?.City,
                    Country = customerBillingInfo?.Address?.Country,
                    NationalIdentificationNumber = customerBillingInfo?.NationalIdentificationNumber,
                    PostalCode = customerBillingInfo?.Address?.PostalCode,
                    StreetName = customerBillingInfo?.Address?.Address,
                    VatIndex = customerBillingInfo?.VatIndex
                };

                await DraftCommands.CreateDraft(
                    customer,
                    creditNote.Date,
                    creditNote.Currency,
                    creditNote.Amount,
                    creditNote.Taxes,
                    creditNote.TotalPrice,
                    creditNote.Description,
                    creditNote.PaymentTerms,
                    creditNote.PurchaseOrderNumber,
                    creditNote.VatIncluded,
                    creditNote.LineItems.Select(MapLineItemForDraft),
                    creditNote.PricesByVat.Select(MapPriceByVatForDraft),
                    creditNote.NonTaxableItems.Select(MapNonTaxableItemForDraft),
                    creditNote.ProvidenceFund?.Description,
                    creditNote.ProvidenceFund?.Rate,
                    creditNote.ProvidenceFund?.Amount,
                    creditNote.WithholdingTax?.Description,
                    creditNote.WithholdingTax?.Rate,
                    creditNote.WithholdingTax?.TaxableAmountRate,
                    creditNote.WithholdingTax?.Amount,
                    creditNote.TotalToPay);

                Snackbar.Add(localizer[nameof(Resources.Pages.IssueOutgoingCreditNote.DraftSaveSuccessMessage)], Severity.Success);
                NavigationManager.NavigateTo(UrlBuilder.BuildSearchDraftsUrl());
            }
            catch
            {
                Snackbar.Add(localizer[nameof(Resources.Pages.IssueOutgoingCreditNote.DraftSaveErrorMessage)], Severity.Error);
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

        private IssueCreditNoteCommand.LineItem MapLineItemForIssue(InvoiceLineItem lineItem)
        {
            return new IssueCreditNoteCommand.LineItem(
                lineItem.Code,
                lineItem.Description,
                lineItem.Quantity,
                lineItem.TotalPrice,
                lineItem.UnitPrice,
                lineItem.Vat?.Rate ?? 0,
                lineItem.Vat?.Description);
        }

        private IssueCreditNoteCommand.PriceByVat MapPriceByVatForIssue(InvoicePriceByVat priceByVat)
        {
            return new IssueCreditNoteCommand.PriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private IssueCreditNoteCommand.NonTaxableItem MapNonTaxableItemForIssue(NonTaxableItem nonTaxableItem)
            => new IssueCreditNoteCommand.NonTaxableItem(nonTaxableItem.Description, nonTaxableItem.Amount);
        #endregion
    }
}
