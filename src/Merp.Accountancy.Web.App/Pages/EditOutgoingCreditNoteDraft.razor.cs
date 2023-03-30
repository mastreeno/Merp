using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Drafts;
using Merp.Accountancy.Drafts.Commands;
using Merp.Accountancy.Drafts.Model;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Services;
using Merp.Registry.Web.Api.Internal;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class EditOutgoingCreditNoteDraft
    {
        [Parameter] public Guid Id { get; set; }

        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] public IDatabase Database { get; set; } = default!;

        [Inject] public IAppContext AppContext { get; set; } = default!;

        [Inject] public IPartyApiServices PartyApi { get; set; } = default!;

        [Inject] public IAccountancySettingsProvider AccountancySettings { get; set; } = default!;

        [Inject] public OutgoingCreditNoteCommands DraftCommands { get; set; } = default!;

        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        private IssueInvoiceForm.ViewModel? model = new();

        private async Task EditDraftAsync(IssueInvoiceForm.ViewModel draft)
        {
            try
            {
                var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(draft.Customer!.OriginalId);
                var customer = new PartyInfo
                {
                    OriginalId = draft.Customer!.OriginalId,
                    Name = draft.Customer!.Name,
                    City = customerBillingInfo?.Address?.City,
                    Country = customerBillingInfo?.Address?.Country,
                    NationalIdentificationNumber = customerBillingInfo?.NationalIdentificationNumber,
                    PostalCode = customerBillingInfo?.Address?.PostalCode,
                    StreetName = customerBillingInfo?.Address?.Address,
                    VatIndex = customerBillingInfo?.VatIndex
                };

                await DraftCommands.EditDraft(
                    Id,
                    customer,
                    draft.Date,
                    draft.Currency,
                    draft.Amount,
                    draft.Taxes,
                    draft.TotalPrice,
                    draft.Description,
                    draft.PaymentTerms,
                    draft.PurchaseOrderNumber,
                    draft.VatIncluded,
                    draft.LineItems.Select(MapLineItemForDraft),
                    draft.PricesByVat.Select(MapPriceByVatForDraft),
                    draft.NonTaxableItems.Select(MapNonTaxableItemForDraft),
                    draft.ProvidenceFund?.Description,
                    draft.ProvidenceFund?.Rate,
                    draft.ProvidenceFund?.Amount,
                    draft.WithholdingTax?.Description,
                    draft.WithholdingTax?.Rate,
                    draft.WithholdingTax?.TaxableAmountRate,
                    draft.WithholdingTax?.Amount,
                    draft.TotalToPay);

                Snackbar.Add("Draft update successfully!", Severity.Success);
            }
            catch
            {
                Snackbar.Add("There was an error updating this draft. Please, try again later", Severity.Error);
            }
        }

        private async Task IssueOutgoingCreditNoteFromDraftAsync(IssueInvoiceForm.ViewModel draft)
        {
            try
            {
                var customerBillingInfo = await PartyApi.GetPartyBillingInfoByPartyIdAsync(draft.Customer!.OriginalId);
                var invoicingSettings = AccountancySettings.GetInvoicingSettings();

                var command = new IssueCreditNoteCommand(
                    AppContext.UserId,
                    draft.Date!.Value,
                    draft.Currency,
                    draft.Amount,
                    draft.Taxes,
                    draft.TotalPrice,
                    draft.TotalToPay,
                    draft.Description,
                    draft.PaymentTerms,
                    draft.PurchaseOrderNumber,
                    draft.Customer.OriginalId,
                    draft.Customer.Name,
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
                    draft.LineItems.Select(MapLineItemForIssue),
                    draft.VatIncluded,
                    draft.PricesByVat.Select(MapPriceByVatForIssue),
                    draft.NonTaxableItems.Select(MapNonTaxableItemForIssue),
                    draft.ProvidenceFund?.Description,
                    draft.ProvidenceFund?.Rate,
                    draft.ProvidenceFund?.Amount,
                    draft.WithholdingTax?.Description,
                    draft.WithholdingTax?.Rate,
                    draft.WithholdingTax?.TaxableAmountRate,
                    draft.WithholdingTax?.Amount);

                await Bus.Send(command);
                //TODO should we remove draft here?!

                Snackbar.Add("Credit note issued correctly from draft!", Severity.Success);
                NavigationManager.NavigateTo(UrlBuilder.BuildSearchInvoicesUrl());
            }
            catch
            {
                Snackbar.Add("Error issuing invoice from draft", Severity.Error);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var draft = await Database.OutgoingCreditNoteDrafts
                .Include(d => d.LineItems)
                .Include(d => d.PricesByVat)
                .Include(d => d.NonTaxableItems)
                .Include(d => d.WithholdingTax)
                .Include(d => d.WithholdingTax)
                .SingleOrDefaultAsync(d => d.Id == Id);

            if (draft is null)
            {
                model = null;
            }
            else
            {
                model = ConvertDraftToViewModel(draft);
            }
        }

        private IssueInvoiceForm.ViewModel? ConvertDraftToViewModel(OutgoingCreditNoteDraft draft)
        {
            var viewModel = new IssueInvoiceForm.ViewModel
            {
                Amount = draft.TaxableAmount,
                Currency = draft.Currency,
                Customer = new PartyInfoAutocomplete.ViewModel
                {
                    OriginalId = draft.Customer.OriginalId,
                    Name = draft.Customer.Name
                },
                Date = draft.Date,
                Description = draft.Description,
                TotalPrice = draft.TotalPrice,
                Taxes = draft.Taxes,
                PaymentTerms = draft.PaymentTerms,
                PurchaseOrderNumber = draft.PurchaseOrderNumber,
                TotalToPay = draft.TotalToPay,
                VatIncluded = draft.PricesAreVatIncluded,
                LineItems = draft.LineItems.Select(MapDraftLineItemToInvoiceLineItem).ToList(),
                PricesByVat = draft.PricesByVat.Select(MapDraftPriceByVatToInvoicePriceByVat).ToList(),
                NonTaxableItems = draft.NonTaxableItems.Select(MapDraftNonTaxableItemToInvoiceNonTaxableItem).ToList(),
                ProvidenceFund = draft.ProvidenceFund is null ? null : new Model.ProvidenceFund
                {
                    Amount = draft.ProvidenceFund.Amount ?? 0,
                    Description = draft.ProvidenceFund.Description,
                    Rate = draft.ProvidenceFund.Rate ?? 0,
                },
                WithholdingTax = draft.WithholdingTax is null ? null : new Model.WithholdingTax
                {
                    Amount = draft.WithholdingTax.Amount,
                    Description = draft.WithholdingTax.Description,
                    Rate = draft.WithholdingTax.Rate,
                    TaxableAmountRate = draft.WithholdingTax.TaxableAmountRate
                }
            };

            return viewModel;
        }

        #region Mapping
        private Model.InvoiceLineItem MapDraftLineItemToInvoiceLineItem(DraftLineItem lineItem)
        {
            return new Model.InvoiceLineItem
            {
                Code = lineItem.Code,
                Description = lineItem.Description,
                Quantity = lineItem.Quantity,
                TotalPrice = lineItem.TotalPrice,
                UnitPrice = lineItem.UnitPrice,
                Vat = new Model.Vat { Rate = lineItem.Vat, Description = lineItem.VatDescription }
            };
        }

        private Model.InvoicePriceByVat MapDraftPriceByVatToInvoicePriceByVat(PriceByVat priceByVat)
        {
            return new Model.InvoicePriceByVat
            {
                ProvidenceFundAmount = priceByVat.ProvidenceFundAmount ?? 0,
                TaxableAmount = priceByVat.TaxableAmount,
                TotalPrice = priceByVat.TotalPrice,
                VatAmount = priceByVat.VatAmount,
                VatRate = priceByVat.VatRate
            };
        }

        private Model.NonTaxableItem MapDraftNonTaxableItemToInvoiceNonTaxableItem(NonTaxableItem nonTaxableItem)
        {
            return new Model.NonTaxableItem
            {
                Amount = nonTaxableItem.Amount,
                Description = nonTaxableItem.Description
            };
        }

        private DraftLineItem MapLineItemForDraft(Model.InvoiceLineItem lineItem)
        {
            return new DraftLineItem
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

        private PriceByVat MapPriceByVatForDraft(Model.InvoicePriceByVat priceByVat)
        {
            return new PriceByVat
            {
                ProvidenceFundAmount = priceByVat.ProvidenceFundAmount,
                TaxableAmount = priceByVat.TaxableAmount,
                TotalPrice = priceByVat.TotalPrice,
                VatAmount = priceByVat.VatAmount,
                VatRate = priceByVat.VatRate
            };
        }

        private NonTaxableItem MapNonTaxableItemForDraft(Model.NonTaxableItem nonTaxableItem)
        {
            return new NonTaxableItem
            {
                Amount = nonTaxableItem.Amount,
                Description = nonTaxableItem.Description
            };
        }

        private IssueCreditNoteCommand.LineItem MapLineItemForIssue(Model.InvoiceLineItem lineItem)
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

        private IssueCreditNoteCommand.PriceByVat MapPriceByVatForIssue(Model.InvoicePriceByVat priceByVat)
        {
            return new IssueCreditNoteCommand.PriceByVat(
                priceByVat.TaxableAmount,
                priceByVat.VatRate,
                priceByVat.VatAmount,
                priceByVat.TotalPrice,
                priceByVat.ProvidenceFundAmount);
        }

        private IssueCreditNoteCommand.NonTaxableItem MapNonTaxableItemForIssue(Model.NonTaxableItem nonTaxableItem)
            => new IssueCreditNoteCommand.NonTaxableItem(nonTaxableItem.Description, nonTaxableItem.Amount);
        #endregion
    }
}
