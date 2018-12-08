using Merp.Accountancy.Drafts;
using Merp.Accountancy.Drafts.Commands;
using Merp.Accountancy.Drafts.Model;
using Merp.Accountancy.Web.Models.Draft;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Merp.Accountancy.Web.AccountancyBoundedContextConfigurator;

namespace Merp.Accountancy.Web.WorkerServices
{
    public class DraftControllerWorkerServices
    {
        public OutgoingInvoiceCommands OutgoingInvoiceDraftCommands { get; private set; }

        public OutgoingCreditNoteCommands OutgoingCreditNoteDraftCommands { get; private set; }

        public IDatabase Database { get; private set; }

        public InvoicingSettings Settings { get; set; }

        public DraftControllerWorkerServices(OutgoingInvoiceCommands outgoingInvoiceDraftCommands, OutgoingCreditNoteCommands outgoingCreditNoteDraftCommands, IDatabase database, InvoicingSettings settings)
        {
            OutgoingInvoiceDraftCommands = outgoingInvoiceDraftCommands ?? throw new ArgumentNullException(nameof(outgoingInvoiceDraftCommands));
            OutgoingCreditNoteDraftCommands = outgoingCreditNoteDraftCommands ?? throw new ArgumentNullException(nameof(outgoingCreditNoteDraftCommands));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IEnumerable<DraftCustomerModel> GetDraftCustomers()
        {
            var model = Database.OutgoingInvoiceDrafts
                .Where(d => d.Customer.OriginalId != Guid.Empty)
                .OrderBy(d => d.Customer.Name)
                .Select(d => new DraftCustomerModel
                {
                    Id = d.Customer.OriginalId,
                    Name = d.Customer.Name
                }).ToArray();

            return model;
        }

        public SearchModel SearchDrafts(SearchModel.DraftKind kind, Guid? customerId, DateTime? dateFrom, DateTime? dateTo, int page, int size)
        {
            var model = new SearchModel();

            var drafts = new List<SearchModel.Draft>();

            if (kind == SearchModel.DraftKind.Any || kind == SearchModel.DraftKind.OutgoingInvoice)
            {
                var outgoingInvoiceDrafts = Database.OutgoingInvoiceDrafts;
                if (customerId.HasValue)
                {
                    outgoingInvoiceDrafts = outgoingInvoiceDrafts.Where(d => d.Customer.OriginalId == customerId.Value);
                }
                if (dateFrom.HasValue)
                {
                    outgoingInvoiceDrafts = outgoingInvoiceDrafts.Where(i => i.Date >= dateFrom.Value);
                }
                if (dateTo.HasValue)
                {
                    outgoingInvoiceDrafts = outgoingInvoiceDrafts.Where(i => i.Date <= dateTo.Value);
                }

                drafts.AddRange(outgoingInvoiceDrafts.Select(d => new SearchModel.Draft
                {
                    Id = d.Id,
                    Currency = d.Currency,
                    CustomerName = d.Customer.Name,
                    Date = d.Date,
                    DocumentType = "outgoingInvoice",
                    TotalPrice = d.TotalPrice
                }));
            }

            if (kind == SearchModel.DraftKind.Any || kind == SearchModel.DraftKind.OutgoingCreditNote)
            {
                var outgoingCreditNoteDrafts = Database.OutgoingCreditNoteDrafts;
                if (customerId.HasValue)
                {
                    outgoingCreditNoteDrafts = outgoingCreditNoteDrafts.Where(d => d.Customer.OriginalId == customerId.Value);
                }
                if (dateFrom.HasValue)
                {
                    outgoingCreditNoteDrafts = outgoingCreditNoteDrafts.Where(i => i.Date >= dateFrom.Value);
                }
                if (dateTo.HasValue)
                {
                    outgoingCreditNoteDrafts = outgoingCreditNoteDrafts.Where(i => i.Date <= dateTo.Value);
                }

                drafts.AddRange(outgoingCreditNoteDrafts.Select(d => new SearchModel.Draft
                {
                    Id = d.Id,
                    Currency = d.Currency,
                    CustomerName = d.Customer.Name,
                    Date = d.Date,
                    DocumentType = "outgoingCreditNote",
                    TotalPrice = -d.TotalPrice
                }));
            }

            model.TotalNumberOfDrafts = drafts.Count();

            int skip = (page - 1) * size;

            model.Drafts = drafts
                .OrderByDescending(d => d.Date)
                .Skip(skip)
                .Take(size)
                .ToArray();

            return model;
        }

        public async Task CreateOutgoingDraftAsync(CreateOutgoingDraftModel model)
        {
            var customer = new PartyInfo
            {
                OriginalId = model.Customer.OriginalId,
                Name = model.Customer.Name,
                StreetName = Settings.Address,
                City = Settings.City,
                PostalCode = Settings.PostalCode,
                Country = Settings.Country,
                VatIndex = Settings.TaxId,
                NationalIdentificationNumber = Settings.TaxId
            };

            var lineItems = model.LineItems.Select(l => new DraftLineItem
            {
                Code = l.Code,
                Description = l.Description,
                Quantity = l.Quantity,
                TotalPrice = l.TotalPrice,
                UnitPrice = l.UnitPrice,
                Vat = l.Vat
            });

            var pricesByVat = model.PricesByVat.Select(p => new PriceByVat
            {
                TaxableAmount = p.TaxableAmount,
                TotalPrice = p.TotalPrice,
                VatAmount = p.VatAmount,
                VatRate = p.VatRate
            });

            var nonTaxableItems = model.NonTaxableItems.Select(t => new NonTaxableItem
            {
                Amount = t.Amount,
                Description = t.Description
            });

            switch (model.Type)
            {
                case Models.OutgoingDocumentTypes.OutgoingInvoice:
                    await OutgoingInvoiceDraftCommands.CreateDraft(
                        customer,
                        model.Date,
                        model.Currency,
                        model.Amount,
                        model.Taxes,
                        model.TotalPrice,
                        model.Description,
                        model.PaymentTerms,
                        model.PurchaseOrderNumber,
                        model.VatIncluded,
                        lineItems,
                        pricesByVat,
                        nonTaxableItems);

                    break;
                case Models.OutgoingDocumentTypes.OutgoingCreditNote:
                    await OutgoingCreditNoteDraftCommands.CreateDraft(
                        customer,
                        model.Date,
                        model.Currency,
                        model.Amount,
                        model.Taxes,
                        model.TotalPrice,
                        model.Description,
                        model.PaymentTerms,
                        model.PurchaseOrderNumber,
                        model.VatIncluded,
                        lineItems,
                        pricesByVat,
                        nonTaxableItems);

                    break;
            }
        }

        #region Outgoing invoice Drafts
        public OutgoingInvoiceDraftModel GetEditOutgoingInvoiceDraft(Guid draftId)
        {
            var draft = Database.OutgoingInvoiceDrafts
                .Include(d => d.LineItems)
                .Include(d => d.PricesByVat)
                .Include(d => d.NonTaxableItems)
                .Single(d => d.Id == draftId);

            return new OutgoingInvoiceDraftModel
            {
                Amount = draft.TaxableAmount,
                Currency = draft.Currency,
                Customer = new Models.PartyInfo
                {
                    OriginalId = draft.Customer.OriginalId,
                    Name = draft.Customer.Name
                },
                Date = draft.Date,
                Description = draft.Description,
                LineItems = draft.LineItems.Select(li => new DraftLineItemModel
                {
                    Code = li.Code,
                    Description = li.Description,
                    Id = li.Id,
                    Quantity = li.Quantity,
                    TotalPrice = li.TotalPrice,
                    UnitPrice = li.UnitPrice,
                    Vat = li.Vat
                }),
                NonTaxableItems = draft.NonTaxableItems.Select(nt => new NonTaxableItemModel
                {
                    Amount = nt.Amount,
                    Description = nt.Description,
                    Id = nt.Id
                }),
                PaymentTerms = draft.PaymentTerms,
                PricesByVat = draft.PricesByVat.Select(p => new DraftPriceByVatModel
                {
                    Id = p.Id,
                    TaxableAmount = p.TaxableAmount,
                    TotalPrice = p.TotalPrice,
                    VatAmount = p.VatAmount,
                    VatRate = p.VatRate
                }),
                PurchaseOrderNumber = draft.PurchaseOrderNumber,
                Taxes = draft.Taxes,
                TotalPrice = draft.TotalPrice,
                VatIncluded = draft.PricesAreVatIncluded
            };
        }

        public async Task EditOutgoingInvoiceDraftAsync(Guid draftId, OutgoingInvoiceDraftModel model)
        {
            var customer = new PartyInfo
            {
                OriginalId = model.Customer.OriginalId,
                Name = model.Customer.Name,
                StreetName = Settings.Address,
                City = Settings.City,
                PostalCode = Settings.PostalCode,
                Country = Settings.Country,
                VatIndex = Settings.TaxId,
                NationalIdentificationNumber = Settings.TaxId
            };

            var lineItems = model.LineItems.Select(l => new DraftLineItem
            {
                Id = l.Id,
                Code = l.Code,
                Description = l.Description,
                Quantity = l.Quantity,
                TotalPrice = l.TotalPrice,
                UnitPrice = l.UnitPrice,
                Vat = l.Vat
            });

            var pricesByVat = model.PricesByVat.Select(p => new PriceByVat
            {
                TaxableAmount = p.TaxableAmount,
                TotalPrice = p.TotalPrice,
                VatAmount = p.VatAmount,
                VatRate = p.VatRate
            });

            var nonTaxableItems = model.NonTaxableItems.Select(t => new NonTaxableItem
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description
            });

            await OutgoingInvoiceDraftCommands.EditDraft(
                draftId,
                customer,
                model.Date,
                model.Currency,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.VatIncluded,
                lineItems,
                pricesByVat,
                nonTaxableItems);
        }

        public async Task DeleteOutgoingInvoiceDraftAsync(Guid draftId)
        {
            await OutgoingInvoiceDraftCommands.DeleteDraft(draftId);
        }
        #endregion

        #region Outgoing credit note Drafts
        public OutgoingCreditNoteDraftModel GetEditOutgoingCreditNoteDraft(Guid draftId)
        {
            var draft = Database.OutgoingCreditNoteDrafts
                .Include(d => d.LineItems)
                .Include(d => d.PricesByVat)
                .Include(d => d.NonTaxableItems)
                .Single(d => d.Id == draftId);

            return new OutgoingCreditNoteDraftModel
            {
                Amount = draft.TaxableAmount,
                Currency = draft.Currency,
                Customer = new Models.PartyInfo
                {
                    OriginalId = draft.Customer.OriginalId,
                    Name = draft.Customer.Name
                },
                Date = draft.Date,
                Description = draft.Description,
                LineItems = draft.LineItems.Select(li => new DraftLineItemModel
                {
                    Code = li.Code,
                    Description = li.Description,
                    Id = li.Id,
                    Quantity = li.Quantity,
                    TotalPrice = li.TotalPrice,
                    UnitPrice = li.UnitPrice,
                    Vat = li.Vat
                }),
                NonTaxableItems = draft.NonTaxableItems.Select(nt => new NonTaxableItemModel
                {
                    Amount = nt.Amount,
                    Description = nt.Description,
                    Id = nt.Id
                }),
                PaymentTerms = draft.PaymentTerms,
                PricesByVat = draft.PricesByVat.Select(p => new DraftPriceByVatModel
                {
                    Id = p.Id,
                    TaxableAmount = p.TaxableAmount,
                    TotalPrice = p.TotalPrice,
                    VatAmount = p.VatAmount,
                    VatRate = p.VatRate
                }),
                PurchaseOrderNumber = draft.PurchaseOrderNumber,
                Taxes = draft.Taxes,
                TotalPrice = draft.TotalPrice,
                VatIncluded = draft.PricesAreVatIncluded
            };
        }

        public async Task EditOutgoingCreditNoteDraftAsync(Guid draftId, OutgoingCreditNoteDraftModel model)
        {
            var customer = new PartyInfo
            {
                OriginalId = model.Customer.OriginalId,
                Name = model.Customer.Name,
                StreetName = Settings.Address,
                City = Settings.City,
                PostalCode = Settings.PostalCode,
                Country = Settings.Country,
                VatIndex = Settings.TaxId,
                NationalIdentificationNumber = Settings.TaxId
            };

            var lineItems = model.LineItems.Select(l => new DraftLineItem
            {
                Id = l.Id,
                Code = l.Code,
                Description = l.Description,
                Quantity = l.Quantity,
                TotalPrice = l.TotalPrice,
                UnitPrice = l.UnitPrice,
                Vat = l.Vat
            });

            var pricesByVat = model.PricesByVat.Select(p => new PriceByVat
            {
                TaxableAmount = p.TaxableAmount,
                TotalPrice = p.TotalPrice,
                VatAmount = p.VatAmount,
                VatRate = p.VatRate
            });

            var nonTaxableItems = model.NonTaxableItems.Select(t => new NonTaxableItem
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description
            });

            await OutgoingCreditNoteDraftCommands.EditDraft(
                draftId,
                customer,
                model.Date,
                model.Currency,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.VatIncluded,
                lineItems,
                pricesByVat,
                nonTaxableItems);
        }

        public async Task DeleteOutgoingCreditNoteDraftAsync(Guid draftId)
        {
            await OutgoingCreditNoteDraftCommands.DeleteDraft(draftId);
        }
        #endregion
    }
}
