using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MementoFX.Persistence;
using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Drafts.Commands;
using Merp.Accountancy.QueryStack;
using Merp.Accountancy.QueryStack.Model;
using Merp.Accountancy.Web.Models;
using Merp.Accountancy.Web.Models.Invoice;
using Merp.Domain;
using Microsoft.AspNetCore.Http;
using Rebus.Bus;
using static Merp.Accountancy.Web.AccountancyBoundedContextConfigurator;

namespace Merp.Accountancy.Web.WorkerServices
{
    public class InvoiceControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public IRepository Repository { get; private set; }

        public IBus Bus { get; private set; }

        public InvoicingSettings Settings { get; set; }

        public OutgoingInvoiceCommands OutgoingInvoiceDraftCommands { get; private set; }

        public OutgoingCreditNoteCommands OutgoingCreditNoteDraftCommands { get; private set; }

        public IHttpContextAccessor ContextAccessor { get; private set; }

        public InvoiceControllerWorkerServices(IDatabase database, IBus bus, InvoicingSettings settings, IRepository repository, OutgoingInvoiceCommands outgoingInvoiceDraftCommands, OutgoingCreditNoteCommands outgoingCreditNoteDraftCommands, IHttpContextAccessor contextAccessor)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            OutgoingInvoiceDraftCommands = outgoingInvoiceDraftCommands ?? throw new ArgumentNullException(nameof(outgoingInvoiceDraftCommands));
            OutgoingCreditNoteDraftCommands = outgoingCreditNoteDraftCommands ?? throw new ArgumentNullException(nameof(outgoingCreditNoteDraftCommands));
            ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public SearchModel SearchInvoices(SearchModel.InvoiceKind kind, SearchModel.InvoiceState status, Guid? customerId, Guid? supplierId, DateTime? dateFrom, DateTime? dateTo, int page, int size)
        {
            var invoices = new List<SearchModel.Invoice>();
            var incomingInvoices = Database.IncomingInvoices;
            if (kind == SearchModel.InvoiceKind.Any || kind == SearchModel.InvoiceKind.IncomingInvoices)
            {
                if (status == SearchModel.InvoiceState.Overdue)
                    incomingInvoices = incomingInvoices.Where(i => i.IsOverdue == true);
                else if (status == SearchModel.InvoiceState.Outstanding)
                    incomingInvoices = incomingInvoices.Where(i => i.IsPaid == false);
                else if (status == SearchModel.InvoiceState.Paid)
                    incomingInvoices = incomingInvoices.Where(i => i.IsPaid == true);

                incomingInvoices = FilterInvoicesByDate(incomingInvoices, dateFrom, dateTo);
                if (supplierId.HasValue)
                {
                    incomingInvoices = FilterInvoicesBySupplier(incomingInvoices, supplierId.Value);
                }
                if (customerId.HasValue)
                {
                    incomingInvoices = FilterInvoicesByCustomer(incomingInvoices, customerId.Value);
                }

                invoices.AddRange(incomingInvoices.Select(i => new SearchModel.Invoice { Uid = i.OriginalId, DocumentType = "incomingInvoice", Number = i.Number, Date = i.Date, DueDate = i.DueDate, CustomerName = i.Customer.Name, SupplierName = i.Supplier.Name, TotalPrice = i.TotalPrice, Currency = i.Currency }).OrderByDescending(i => i.Date).Take(20));
            }

            if (kind == SearchModel.InvoiceKind.Any || kind == SearchModel.InvoiceKind.OutgoingInvoices)
            {
                var outgoingInvoices = Database.OutgoingInvoices;
                if (status == SearchModel.InvoiceState.Overdue)
                    outgoingInvoices = outgoingInvoices.Where(i => i.IsOverdue == true);
                else if (status == SearchModel.InvoiceState.Outstanding)
                    outgoingInvoices = outgoingInvoices.Where(i => i.IsPaid == false);
                else if (status == SearchModel.InvoiceState.Paid)
                    outgoingInvoices = outgoingInvoices.Where(i => i.IsPaid == true);

                outgoingInvoices = FilterInvoicesByDate(outgoingInvoices, dateFrom, dateTo);
                if (customerId.HasValue)
                {
                    outgoingInvoices = FilterInvoicesByCustomer(outgoingInvoices, customerId.Value);
                }
                if (supplierId.HasValue)
                {
                    outgoingInvoices = FilterInvoicesBySupplier(outgoingInvoices, supplierId.Value);
                }

                invoices.AddRange(outgoingInvoices.Select(i => new SearchModel.Invoice { Uid = i.OriginalId, DocumentType = "outgoingInvoice", Number = i.Number, Date = i.Date, DueDate = i.DueDate, CustomerName = i.Customer.Name, SupplierName = i.Supplier.Name, TotalPrice = i.TotalPrice, Currency = i.Currency }).OrderByDescending(i => i.Date).Take(20));
            }

            if (kind == SearchModel.InvoiceKind.Any || kind == SearchModel.InvoiceKind.OutgoingCreditNotes)
            {
                var outgoingCreditNotes = Database.OutgoingCreditNotes;
                if (status == SearchModel.InvoiceState.Overdue)
                    outgoingCreditNotes = outgoingCreditNotes.Where(i => i.IsOverdue == true);
                else if (status == SearchModel.InvoiceState.Outstanding)
                    outgoingCreditNotes = outgoingCreditNotes.Where(i => i.IsPaid == false);
                else if (status == SearchModel.InvoiceState.Paid)
                    outgoingCreditNotes = outgoingCreditNotes.Where(i => i.IsPaid == true);

                outgoingCreditNotes = FilterInvoicesByDate(outgoingCreditNotes, dateFrom, dateTo);
                if (customerId.HasValue)
                {
                    outgoingCreditNotes = FilterInvoicesByCustomer(outgoingCreditNotes, customerId.Value);
                }
                if (supplierId.HasValue)
                {
                    outgoingCreditNotes = FilterInvoicesBySupplier(outgoingCreditNotes, supplierId.Value);
                }

                invoices.AddRange(outgoingCreditNotes.Select(i => new SearchModel.Invoice { Uid = i.OriginalId, DocumentType = "outgoingCreditNote", Number = i.Number, Date = i.Date, DueDate = i.DueDate, CustomerName = i.Customer.Name, SupplierName = i.Supplier.Name, TotalPrice = i.TotalPrice, Currency = i.Currency }).OrderByDescending(i => i.Date).Take(20));
            }

            if (kind == SearchModel.InvoiceKind.Any || kind == SearchModel.InvoiceKind.IncomingCreditNotes)
            {
                var incomingCreditNotes = Database.IncomingCreditNotes;
                if (status == SearchModel.InvoiceState.Overdue)
                    incomingCreditNotes = incomingCreditNotes.Where(i => i.IsOverdue == true);
                else if (status == SearchModel.InvoiceState.Outstanding)
                    incomingCreditNotes = incomingCreditNotes.Where(i => i.IsPaid == false);
                else if (status == SearchModel.InvoiceState.Paid)
                    incomingCreditNotes = incomingCreditNotes.Where(i => i.IsPaid == true);

                incomingCreditNotes = FilterInvoicesByDate(incomingCreditNotes, dateFrom, dateTo);
                if (customerId.HasValue)
                {
                    incomingCreditNotes = FilterInvoicesByCustomer(incomingCreditNotes, customerId.Value);
                }
                if (supplierId.HasValue)
                {
                    incomingCreditNotes = FilterInvoicesBySupplier(incomingCreditNotes, supplierId.Value);
                }

                invoices.AddRange(incomingCreditNotes.Select(i => new SearchModel.Invoice { Uid = i.OriginalId, DocumentType = "incomingCreditNote", Number = i.Number, Date = i.Date, DueDate = i.DueDate, CustomerName = i.Customer.Name, SupplierName = i.Supplier.Name, TotalPrice = i.TotalPrice, Currency = i.Currency }).OrderByDescending(i => i.Date).Take(20));
            }

            int totalNumberOfInvoices = invoices.Count;
            int skip = (page - 1) * size;

            var model = new SearchModel()
            {
                Invoices = invoices.OrderByDescending(i => i.Date).Skip(skip).Take(size),
                TotalNumberOfInvoices = totalNumberOfInvoices
            };
            return model;
        }

        public IEnumerable<InvoiceCustomerModel> GetInvoiceCustomers()
        {
            var model = new List<InvoiceCustomerModel>();

            var outgoingInvoices = Database.OutgoingInvoices
                .OrderBy(i => i.Customer.Name)
                .Select(i => new InvoiceCustomerModel
                {
                    Id = i.Customer.OriginalId,
                    Name = i.Customer.Name
                }).Distinct();

            if (outgoingInvoices.Any())
            {
                model.AddRange(outgoingInvoices);
            }

            var outgoingCreditNotes = Database.OutgoingCreditNotes
                .OrderBy(c => c.Customer.Name)
                .Select(c => new InvoiceCustomerModel
                {
                    Id = c.Customer.OriginalId,
                    Name = c.Customer.Name
                }).Distinct();

            if (outgoingCreditNotes.Any())
            {
                model.AddRange(outgoingCreditNotes);
            }

            return model.OrderBy(i => i.Name);
        }

        public IEnumerable<InvoiceSupplierModel> GetInvoiceSuppliers()
        {
            var model = new List<InvoiceSupplierModel>();

            var incomingInvoices = Database.IncomingInvoices
                .OrderBy(i => i.Supplier.Name)
                .Select(i => new InvoiceSupplierModel
                {
                    Id = i.Supplier.OriginalId,
                    Name = i.Supplier.Name
                }).Distinct();

            if (incomingInvoices.Any())
            {
                model.AddRange(incomingInvoices);
            }

            var incomingCreditNotes = Database.IncomingCreditNotes
                .OrderBy(c => c.Supplier.Name)
                .Select(c => new InvoiceSupplierModel
                {
                    Id = c.OriginalId,
                    Name = c.Supplier.Name
                }).Distinct();

            if (incomingCreditNotes.Any())
            {
                model.AddRange(incomingCreditNotes);
            }

            return model.OrderBy(i => i.Name);
        }

        public InvoicesStatsModel GetInvoicesStats()
        {
            var model = new InvoicesStatsModel
            {
                OutstandingIncomingInvoicesCount = Database.IncomingInvoices.Outstanding().Count(),
                OverdueIncomingInvoicesCount = Database.IncomingInvoices.Overdue().Count(),
                OutstandingIncomingInvoicesTotalPrice = Database.IncomingInvoices.Outstanding().Sum(i => i.TotalPrice),
                OverdueIncomingInvoicesTotalPrice = Database.IncomingInvoices.Overdue().Sum(i => i.TotalPrice),
                OutstandingOutgoingInvoicesCount = Database.OutgoingInvoices.Outstanding().Count(),
                OverdueOutgoingInvoicesCount = Database.OutgoingInvoices.Overdue().Count(),
                OutstandingOutgoingInvoicesTotalPrice = Database.OutgoingInvoices.Outstanding().Sum(i => i.TotalPrice),
                OverdueOutgoingInvoicesTotalPrice = Database.OutgoingInvoices.Overdue().Sum(i => i.TotalPrice)
            };

            return model;
        }

        #region Outgoing Invoices
        public async Task IssueOutgoingInvoiceAsync(IssueOutgoingInvoiceModel model)
        {
            var userId = GetCurrentUserId();
            MerpCommand command = null;

            switch (model.Type)
            {
                case Models.OutgoingDocumentTypes.OutgoingInvoice:
                    command = BuildIssueInvoiceCommandFromModel(model, userId);
                    break;
                case Models.OutgoingDocumentTypes.OutgoingCreditNote:
                    command = BuildIssueCreditNoteCommandFromModel(model, userId);
                    break;
            }
            
            await Bus.Send(command);
        }

        public async Task IssueOutgoingCreditNoteFromDraftAsync(IssueOutgoingCreditNoteFromDraftModel model)
        {
            var userId = GetCurrentUserId();

            await OutgoingCreditNoteDraftCommands.DeleteDraft(model.DraftId);

            var invoiceLineItems = model.LineItems.Select(i => new IssueCreditNoteCommand.LineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    -1 * CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    -1 * CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new IssueCreditNoteCommand.PriceByVat(
                -p.TaxableAmount,
                p.VatRate,
                -p.VatAmount,
                -p.TotalPrice,
                p.ProvidenceFundAmount.HasValue ? -p.ProvidenceFundAmount : null)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new IssueCreditNoteCommand.NonTaxableItem(t.Description, t.Amount)).ToArray();

            var command = new IssueCreditNoteCommand(
                userId,
                model.Date,
                model.Currency,
                -model.Amount,
                -model.Taxes,
                -model.TotalPrice,
                -model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                Settings.TaxId,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund == null ? null : -model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax == null ? null : -model.WithholdingTax?.Amount);

            await Bus.Send(command);
        }

        public async Task IssueOutgoingInvoiceFromDraftAsync(IssueOutgoingInvoiceFromDraftModel model)
        {
            var userId = GetCurrentUserId();

            await OutgoingInvoiceDraftCommands.DeleteDraft(model.DraftId);

            var invoiceLineItems = model.LineItems.Select(i => new IssueInvoiceCommand.InvoiceLineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new IssueInvoiceCommand.InvoicePriceByVat(
                p.TaxableAmount,
                p.VatRate,
                p.VatAmount,
                p.TotalPrice,
                p.ProvidenceFundAmount)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new IssueInvoiceCommand.NonTaxableItem(t.Description, t.Amount)).ToArray();

            var command = new IssueInvoiceCommand(
                userId,
                model.Date,
                model.Currency,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                Settings.TaxId,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax?.Amount);

            await Bus.Send(command);
        }

        public OutgoingInvoicesNotLinkedToAJobOrderModel GetOutgoingInvoicesNotLinkedToAJobOrder(OutgoingDocumentTypes? type, string customer, int page, int size)
        {
            var invoicesModel = new List<OutgoingInvoicesNotLinkedToAJobOrderModel.Invoice>();

            if (type == null || type == OutgoingDocumentTypes.OutgoingInvoice)
            {
                var invoices = Database.OutgoingInvoices.NotAssociatedToAnyJobOrder();
                if (!string.IsNullOrWhiteSpace(customer))
                {
                    invoices = invoices.Where(i => i.Customer.Name.Contains(customer));
                }

                invoicesModel.AddRange((from i in invoices
                                        orderby i.Date
                                        select new OutgoingInvoicesNotLinkedToAJobOrderModel.Invoice
                                        {
                                            Amount = i.TotalPrice,
                                            Date = i.Date,
                                            Number = i.Number,
                                            CustomerName = i.Customer.Name,
                                            OriginalId = i.OriginalId,
                                            Type = Models.OutgoingDocumentTypes.OutgoingInvoice.ToString()
                                        }).ToArray());
            }

            if (type == null || type == OutgoingDocumentTypes.OutgoingCreditNote)
            {
                var creditNotes = Database.OutgoingCreditNotes.NotAssociatedToAnyJobOrder();
                if (!string.IsNullOrWhiteSpace(customer))
                {
                    creditNotes = creditNotes.Where(c => c.Customer.Name.Contains(customer));
                }

                invoicesModel.AddRange((from c in creditNotes
                                        orderby c.Date
                                        select new OutgoingInvoicesNotLinkedToAJobOrderModel.Invoice
                                        {
                                            Amount = c.TotalPrice,
                                            Date = c.Date,
                                            Number = c.Number,
                                            CustomerName = c.Customer.Name,
                                            OriginalId = c.OriginalId,
                                            Type = Models.OutgoingDocumentTypes.OutgoingCreditNote.ToString()
                                        }).ToArray());
            }
            

            int totalNumberOfInvoices = invoicesModel.Count;
            int skip = (page - 1) * size;

            return new OutgoingInvoicesNotLinkedToAJobOrderModel
            {
                TotalNumberOfInvoices = totalNumberOfInvoices,
                Invoices = invoicesModel
                    .OrderBy(i => i.Date)
                    .Skip(skip)
                    .Take(size)
            };
        }

        public async Task LinkOutgoingInvoiceToJobOrderAsync(Guid invoiceId, LinkOutgoingInvoiceToJobOrderModel model)
        {
            var userId = GetCurrentUserId();
            MerpCommand command = null;

            var jobOrderId = (from j in Database.JobOrders
                              where j.Number == model.JobOrderNumber
                              select j.OriginalId).Single();

            switch (model.Type)
            {
                case Models.OutgoingDocumentTypes.OutgoingInvoice:
                    command = new LinkOutgoingInvoiceToJobOrderCommand(userId, invoiceId, jobOrderId, model.DateOfLink, model.Amount);
                    break;
                case Models.OutgoingDocumentTypes.OutgoingCreditNote:
                    command = new LinkOutgoingCreditNoteToJobOrderCommand(userId, invoiceId, jobOrderId, model.DateOfLink, model.Amount);
                    break;
            }
            
            await Bus.Send(command);
        }

        public OutgoingCreditNoteDetailsModel GetOutgoingCreditNoteDetails(Guid creditNoteId)
        {
            var invoice = Repository.GetById<CommandStack.Model.OutgoingCreditNote>(creditNoteId);

            return new OutgoingCreditNoteDetailsModel
            {
                Customer = new Models.BillingInfo
                {
                    Id = invoice.Customer.Id,
                    Address = invoice.Customer.StreetName,
                    City = invoice.Customer.City,
                    Country = invoice.Customer.Country,
                    Name = invoice.Customer.Name,
                    NationalIdentificationNumber = invoice.Customer.NationalIdentificationNumber,
                    PostalCode = invoice.Customer.PostalCode,
                    VatNumber = invoice.Customer.VatIndex
                },
                Supplier = new Models.BillingInfo
                {
                    Address = Settings.Address,
                    City = Settings.City,
                    Country = Settings.Country,
                    Id = Guid.Empty,
                    Name = Settings.CompanyName,
                    NationalIdentificationNumber = string.Empty,
                    PostalCode = Settings.PostalCode,
                    VatNumber = string.Empty
                },
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                Date = invoice.Date,
                Description = invoice.Description,
                CreditNoteNumber = invoice.Number,
                IsOverdue = invoice.IsOverdue,
                PaymentDate = invoice.PaymentDate,
                PaymentTerms = invoice.PaymentTerms,
                PurchaseOrderNumber = invoice.PurchaseOrderNumber,
                Taxes = invoice.Taxes,
                TotalPrice = invoice.TotalPrice,
                TotalToPay = invoice.TotalToPay,
                LineItems = invoice.InvoiceLineItems.Select(i => new InvoiceLineItemModel
                {
                    Code = i.Code,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    Vat = i.Vat,
                    VatDescription = i.VatDescription
                }),
                NonTaxableItems = invoice.NonTaxableItems.Select(t => new NonTaxableItemModel
                {
                    Description = t.Description,
                    Amount = t.Amount
                }),
                ProvidenceFund = invoice.ProvidenceFund == null ? null : new ProvidenceFundModel
                {
                    Amount = invoice.ProvidenceFund.Amount,
                    Description = invoice.ProvidenceFund.Description,
                    Rate = invoice.ProvidenceFund.Rate
                },
                WithholdingTax = invoice.WithholdingTax == null ? null : new WithholdingTaxModel
                {
                    Amount = invoice.WithholdingTax.Amount,
                    Description = invoice.WithholdingTax.Description,
                    Rate = invoice.WithholdingTax.Rate,
                    TaxableAmountRate = invoice.WithholdingTax.TaxableAmountRate
                }
            };
        }

        public OutgoingInvoiceDetailsModel GetOutgoingInvoiceDetails(Guid invoiceId)
        {
            var invoice = Repository.GetById<CommandStack.Model.OutgoingInvoice>(invoiceId);

            return new OutgoingInvoiceDetailsModel
            {
                Customer = new Models.BillingInfo
                {
                    Id = invoice.Customer.Id,
                    Address = invoice.Customer.StreetName,
                    City = invoice.Customer.City,
                    Country = invoice.Customer.Country,
                    Name = invoice.Customer.Name,
                    NationalIdentificationNumber = invoice.Customer.NationalIdentificationNumber,
                    PostalCode = invoice.Customer.PostalCode,
                    VatNumber = invoice.Customer.VatIndex
                },
                Supplier = new Models.BillingInfo
                {
                    Address = Settings.Address,
                    City = Settings.City,
                    Country = Settings.Country,
                    Id = Guid.Empty,
                    Name = Settings.CompanyName,
                    NationalIdentificationNumber = string.Empty,
                    PostalCode = Settings.PostalCode,
                    VatNumber = string.Empty
                },
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                Date = invoice.Date,
                Description = invoice.Description,
                DueDate = invoice.DueDate,
                InvoiceNumber = invoice.Number,
                IsOverdue = invoice.IsOverdue,
                PaymentDate = invoice.PaymentDate,
                PaymentTerms = invoice.PaymentTerms,
                PurchaseOrderNumber = invoice.PurchaseOrderNumber,
                Taxes = invoice.Taxes,
                TotalPrice = invoice.TotalPrice,
                TotalToPay = invoice.TotalToPay,
                LineItems = invoice.InvoiceLineItems.Select(i => new InvoiceLineItemModel
                {
                    Code = i.Code,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    Vat = i.Vat,
                    VatDescription = i.VatDescription
                }),
                NonTaxableItems = invoice.NonTaxableItems.Select(t => new NonTaxableItemModel
                {
                    Description = t.Description,
                    Amount = t.Amount
                }),
                ProvidenceFund = invoice.ProvidenceFund == null ? null : new ProvidenceFundModel
                {
                    Amount = invoice.ProvidenceFund.Amount,
                    Description = invoice.ProvidenceFund.Description,
                    Rate = invoice.ProvidenceFund.Rate
                },
                WithholdingTax = invoice.WithholdingTax == null ? null : new WithholdingTaxModel
                {
                    Amount = invoice.WithholdingTax.Amount,
                    Description = invoice.WithholdingTax.Description,
                    Rate = invoice.WithholdingTax.Rate,
                    TaxableAmountRate = invoice.WithholdingTax.TaxableAmountRate
                }
            };
        }

        public async Task RegisterOutgoingInvoiceAsync(RegisterOutgoingInvoiceModel model)
        {
            var userId = GetCurrentUserId();
            MerpCommand command = null;

            switch (model.Type)
            {
                case Models.OutgoingDocumentTypes.OutgoingInvoice:
                    command = BuildRegisterOutgoingInvoiceCommandFromModel(model, userId);
                    break;
                case Models.OutgoingDocumentTypes.OutgoingCreditNote:
                    command = BuildRegisterOutgoingCreditNoteCommandFromModel(model, userId);
                    break;
            }

            await Bus.Send(command);
        }
        #endregion

        #region Incoming Invoices
        public async Task RegisterIncomingInvoiceAsync(RegisterIncomingInvoiceModel model)
        {
            var userId = GetCurrentUserId();
            MerpCommand command = null;

            switch (model.Type)
            {
                case Models.IncomingDocumentTypes.IncomingInvoice:
                    command = BuildRegisterIncomingInvoiceCommandFromModel(model, userId);
                    break;
                case Models.IncomingDocumentTypes.IncomingCreditNote:
                    command = BuildRegisterIncomingCreditNoteCommandFromModel(model, userId);
                    break;
            }

            await Bus.Send(command);
        }

        public IncomingInvoicesNotLinkedToAJobOrderModel GetIncomingInvoicesNotLinkedToAJobOrder(IncomingDocumentTypes? type, string supplier, int page, int size)
        {
            var invoicesModel = new List<IncomingInvoicesNotLinkedToAJobOrderModel.Invoice>();

            if (type == null || type == IncomingDocumentTypes.IncomingInvoice)
            {
                var invoices = Database.IncomingInvoices.NotAssociatedToAnyJobOrder();
                if (!string.IsNullOrWhiteSpace(supplier))
                {
                    invoices = invoices.Where(i => i.Supplier.Name.Contains(supplier));
                }

                invoicesModel.AddRange((from i in invoices
                                        orderby i.Date
                                        select new IncomingInvoicesNotLinkedToAJobOrderModel.Invoice
                                        {
                                            Amount = i.TotalPrice,
                                            Date = i.Date,
                                            Number = i.Number,
                                            SupplierName = i.Supplier.Name,
                                            OriginalId = i.OriginalId,
                                            Type = IncomingDocumentTypes.IncomingInvoice.ToString()
                                        }).ToArray());
            }

            if (type == null || type == IncomingDocumentTypes.IncomingCreditNote)
            {
                var creditNotes = Database.IncomingCreditNotes.NotAssociatedToAnyJobOrder();
                if (!string.IsNullOrWhiteSpace(supplier))
                {
                    creditNotes = creditNotes.Where(c => c.Supplier.Name.Contains(supplier));
                }

                invoicesModel.AddRange((from c in creditNotes
                                        orderby c.Date
                                        select new IncomingInvoicesNotLinkedToAJobOrderModel.Invoice
                                        {
                                            Amount = c.TotalPrice,
                                            Date = c.Date,
                                            Number = c.Number,
                                            SupplierName = c.Supplier.Name,
                                            OriginalId = c.OriginalId,
                                            Type = IncomingDocumentTypes.IncomingCreditNote.ToString()
                                        }).ToArray());
            }
            
            int totalNumberOfInvoices = invoicesModel.Count;
            int skip = (page - 1) * size;

            return new IncomingInvoicesNotLinkedToAJobOrderModel
            {
                TotalNumberOfInvoices = totalNumberOfInvoices,
                Invoices = invoicesModel
                    .OrderBy(i => i.Date)
                    .Skip(skip)
                    .Take(size)
            };
        }

        public async Task LinkIncomingInvoiceToJobOrderAsync(Guid invoiceId, LinkIncomingInvoiceToJobOrderModel model)
        {
            var userId = GetCurrentUserId();
            MerpCommand command = null;

            var jobOrderId = (from j in Database.JobOrders
                              where j.Number == model.JobOrderNumber
                              select j.OriginalId).Single();

            switch (model.Type)
            {
                case IncomingDocumentTypes.IncomingInvoice:
                    command = new LinkIncomingInvoiceToJobOrderCommand(userId, invoiceId, jobOrderId, model.DateOfLink, model.Amount);
                    break;
                case IncomingDocumentTypes.IncomingCreditNote:
                    command = new LinkIncomingCreditNoteToJobOrderCommand(userId, invoiceId, jobOrderId, model.DateOfLink, model.Amount);
                    break;
            }
            
            await Bus.Send(command);
        }

        public IncomingCreditNoteDetailsModel GetIncomingCreditNoteDetails(Guid creditNoteId)
        {
            var creditNote = Repository.GetById<CommandStack.Model.IncomingCreditNote>(creditNoteId);

            return new IncomingCreditNoteDetailsModel
            {
                Customer = new Models.BillingInfo
                {
                    Address = Settings.Address,
                    City = Settings.City,
                    Country = Settings.Country,
                    Id = Guid.Empty,
                    Name = Settings.CompanyName,
                    NationalIdentificationNumber = string.Empty,
                    PostalCode = Settings.PostalCode,
                    VatNumber = string.Empty
                },
                Supplier = new Models.BillingInfo
                {
                    Address = creditNote.Supplier.StreetName,
                    City = creditNote.Supplier.City,
                    Country = creditNote.Supplier.Country,
                    Id = creditNote.Supplier.Id,
                    Name = creditNote.Supplier.Name,
                    NationalIdentificationNumber = creditNote.Supplier.NationalIdentificationNumber,
                    PostalCode = creditNote.Supplier.PostalCode,
                    VatNumber = creditNote.Supplier.VatIndex
                },
                Amount = creditNote.Amount,
                Currency = creditNote.Currency,
                Date = creditNote.Date,
                Description = creditNote.Description,
                DueDate = creditNote.DueDate,
                CreditNoteNumber = creditNote.Number,
                IsOverdue = creditNote.IsOverdue,
                PaymentDate = creditNote.PaymentDate,
                PaymentTerms = creditNote.PaymentTerms,
                PurchaseOrderNumber = creditNote.PurchaseOrderNumber,
                Taxes = creditNote.Taxes,
                TotalPrice = creditNote.TotalPrice,
                TotalToPay = creditNote.TotalToPay,
                LineItems = creditNote.InvoiceLineItems.Select(i => new InvoiceLineItemModel
                {
                    Code = i.Code,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    Vat = i.Vat,
                    VatDescription = i.VatDescription
                }),
                NonTaxableItems = creditNote.NonTaxableItems.Select(t => new NonTaxableItemModel
                {
                    Description = t.Description,
                    Amount = t.Amount
                }),
                ProvidenceFund = creditNote.ProvidenceFund == null ? null : new ProvidenceFundModel
                {
                    Amount = creditNote.ProvidenceFund.Amount,
                    Description = creditNote.ProvidenceFund.Description,
                    Rate = creditNote.ProvidenceFund.Rate
                },
                WithholdingTax = creditNote.WithholdingTax == null ? null : new WithholdingTaxModel
                {
                    Amount = creditNote.WithholdingTax.Amount,
                    Description = creditNote.WithholdingTax.Description,
                    Rate = creditNote.WithholdingTax.Rate,
                    TaxableAmountRate = creditNote.WithholdingTax.TaxableAmountRate
                }
            };
        }

        public IncomingInvoiceDetailsModel GetIncomingInvoiceDetails(Guid invoiceId)
        {
            var invoice = Repository.GetById<CommandStack.Model.IncomingInvoice>(invoiceId);

            return new IncomingInvoiceDetailsModel
            {
                Customer = new Models.BillingInfo
                {
                    Address = Settings.Address,
                    City = Settings.City,
                    Country = Settings.Country,
                    Id = Guid.Empty,
                    Name = Settings.CompanyName,
                    NationalIdentificationNumber = string.Empty,
                    PostalCode = Settings.PostalCode,
                    VatNumber = string.Empty
                },
                Supplier = new Models.BillingInfo
                {
                    Address = invoice.Supplier.StreetName,
                    City = invoice.Supplier.City,
                    Country = invoice.Supplier.Country,
                    Id = invoice.Supplier.Id,
                    Name = invoice.Supplier.Name,
                    NationalIdentificationNumber = invoice.Supplier.NationalIdentificationNumber,
                    PostalCode = invoice.Supplier.PostalCode,
                    VatNumber = invoice.Supplier.VatIndex
                },
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                Date = invoice.Date,
                Description = invoice.Description,
                DueDate = invoice.DueDate,
                InvoiceNumber = invoice.Number,
                IsOverdue = invoice.IsOverdue,
                PaymentDate = invoice.PaymentDate,
                PaymentTerms = invoice.PaymentTerms,
                PurchaseOrderNumber = invoice.PurchaseOrderNumber,
                Taxes = invoice.Taxes,
                TotalPrice = invoice.TotalPrice,
                TotalToPay = invoice.TotalToPay,
                LineItems = invoice.InvoiceLineItems.Select(i => new InvoiceLineItemModel
                {
                    Code = i.Code,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    Vat = i.Vat,
                    VatDescription = i.VatDescription
                }),
                NonTaxableItems = invoice.NonTaxableItems.Select(t => new NonTaxableItemModel
                {
                    Description = t.Description,
                    Amount = t.Amount
                }),
                ProvidenceFund = invoice.ProvidenceFund == null ? null : new ProvidenceFundModel
                {
                    Amount = invoice.ProvidenceFund.Amount,
                    Description = invoice.ProvidenceFund.Description,
                    Rate = invoice.ProvidenceFund.Rate
                },
                WithholdingTax = invoice.WithholdingTax == null ? null : new WithholdingTaxModel
                {
                    Amount = invoice.WithholdingTax.Amount,
                    Description = invoice.WithholdingTax.Description,
                    Rate = invoice.WithholdingTax.Rate,
                    TaxableAmountRate = invoice.WithholdingTax.TaxableAmountRate
                }
            };
        }
        #endregion

        #region Private helpers
        private Guid GetCurrentUserId()
        {
            var userId = ContextAccessor.HttpContext.User.FindFirstValue("sub");
            return Guid.Parse(userId);
        }

        private IQueryable<TInvoice> FilterInvoicesBySupplier<TInvoice>(IQueryable<TInvoice> invoices, Guid supplierId) where TInvoice : Invoice
        {
            return invoices.Where(i => i.Supplier.OriginalId == supplierId);
        }

        private IQueryable<TInvoice> FilterInvoicesByCustomer<TInvoice>(IQueryable<TInvoice> invoices, Guid customerId) where TInvoice : Invoice
        {
            return invoices.Where(i => i.Customer.OriginalId == customerId);
        }

        private IQueryable<TInvoice> FilterInvoicesByDate<TInvoice>(IQueryable<TInvoice> invoices, DateTime? dateFrom, DateTime? dateTo) where TInvoice : Invoice
        {
            if (dateFrom.HasValue)
            {
                invoices = invoices.Where(i => i.Date >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                invoices = invoices.Where(i => i.Date <= dateTo.Value);
            }

            return invoices;
        }

        private decimal CalculatePriceByVat(decimal price, decimal vatPercentage, bool vatIncluded)
        {
            if (!vatIncluded)
            {
                return price;
            }

            var vat = (double)vatPercentage / 100.00;
            return Math.Round(price / (1 + (decimal)vat), 2);
        }

        private IssueInvoiceCommand BuildIssueInvoiceCommandFromModel(IssueOutgoingInvoiceModel model, Guid userId)
        {
            var invoiceLineItems = model.LineItems.Select(i => new IssueInvoiceCommand.InvoiceLineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new IssueInvoiceCommand.InvoicePriceByVat(
                p.TaxableAmount,
                p.VatRate,
                p.VatAmount,
                p.TotalPrice,
                p.ProvidenceFundAmount)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new IssueInvoiceCommand.NonTaxableItem(t.Description, t.Amount)).ToArray();

            var command = new IssueInvoiceCommand(
                userId,
                model.Date,
                model.Currency,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                Settings.TaxId,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax?.Amount);

            return command;
        }

        private IssueCreditNoteCommand BuildIssueCreditNoteCommandFromModel(IssueOutgoingInvoiceModel model, Guid userId)
        {
            var invoiceLineItems = model.LineItems.Select(i => new IssueCreditNoteCommand.LineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    -1 * CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    -1 * CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new IssueCreditNoteCommand.PriceByVat(
                -p.TaxableAmount,
                p.VatRate,
                -p.VatAmount,
                -p.TotalPrice,
                p.ProvidenceFundAmount.HasValue ? -p.ProvidenceFundAmount : null)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new IssueCreditNoteCommand.NonTaxableItem(t.Description, -t.Amount)).ToArray();

            var command = new IssueCreditNoteCommand(
                userId,
                model.Date,
                model.Currency,
                -model.Amount,
                -model.Taxes,
                -model.TotalPrice,
                -model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                Settings.TaxId,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund == null ? null : -model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax == null ? null : -model.WithholdingTax?.Amount);

            return command;
        }

        private RegisterIncomingInvoiceCommand BuildRegisterIncomingInvoiceCommandFromModel(RegisterIncomingInvoiceModel model, Guid userId)
        {
            var invoiceLineItems = model.LineItems.Select(i => new RegisterIncomingInvoiceCommand.InvoiceLineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new RegisterIncomingInvoiceCommand.InvoicePriceByVat(
                p.TaxableAmount,
                p.VatRate,
                p.VatAmount,
                p.TotalPrice,
                p.ProvidenceFundAmount)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new RegisterIncomingInvoiceCommand.NonTaxableItem(t.Description, t.Amount)).ToArray();

            var command = new RegisterIncomingInvoiceCommand(
                userId,
                model.InvoiceNumber,
                model.Date,
                model.DueDate,
                model.Currency,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                Guid.Empty,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                model.Supplier.OriginalId,
                model.Supplier.Name,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax?.Amount);

            return command;
        }

        private RegisterIncomingCreditNoteCommand BuildRegisterIncomingCreditNoteCommandFromModel(RegisterIncomingInvoiceModel model, Guid userId)
        {
            var invoiceLineItems = model.LineItems.Select(i => new RegisterIncomingCreditNoteCommand.LineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    -1 * CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    -1 * CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new RegisterIncomingCreditNoteCommand.PriceByVat(
                -p.TaxableAmount,
                p.VatRate,
                -p.VatAmount,
                -p.TotalPrice,
                p.ProvidenceFundAmount == null ? null : -p.ProvidenceFundAmount)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new RegisterIncomingCreditNoteCommand.NonTaxableItem(t.Description, t.Amount)).ToArray();

            var command = new RegisterIncomingCreditNoteCommand(
                userId,
                model.InvoiceNumber,
                model.Date,
                model.Currency,
                -model.Amount,
                -model.Taxes,
                -model.TotalPrice,
                -model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                Guid.Empty,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                model.Supplier.OriginalId,
                model.Supplier.Name,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund == null ? null : -model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax == null ? null : -model.WithholdingTax?.Amount);

            return command;
        }

        private RegisterOutgoingInvoiceCommand BuildRegisterOutgoingInvoiceCommandFromModel(RegisterOutgoingInvoiceModel model, Guid userId)
        {
            var invoiceLineItems = model.LineItems.Select(i => new RegisterOutgoingInvoiceCommand.InvoiceLineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new RegisterOutgoingInvoiceCommand.InvoicePriceByVat(
                p.TaxableAmount,
                p.VatRate,
                p.VatAmount,
                p.TotalPrice,
                p.ProvidenceFundAmount)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new RegisterOutgoingInvoiceCommand.NonTaxableItem(t.Description, t.Amount)).ToArray();

            var command = new RegisterOutgoingInvoiceCommand(
                userId,
                model.Number,
                model.Date,
                model.DueDate,
                model.Currency,
                model.Amount,
                model.Taxes,
                model.TotalPrice,
                model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                Settings.TaxId,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax?.Amount);

            return command;
        }

        private RegisterOutgoingCreditNoteCommand BuildRegisterOutgoingCreditNoteCommandFromModel(RegisterOutgoingInvoiceModel model, Guid userId)
        {
            var invoiceLineItems = model.LineItems.Select(i => new RegisterOutgoingCreditNoteCommand.LineItem(
                    i.Code,
                    i.Description,
                    i.Quantity,
                    -1 * CalculatePriceByVat(i.UnitPrice, i.Vat, model.VatIncluded),
                    -1 * CalculatePriceByVat(i.TotalPrice, i.Vat, model.VatIncluded),
                    i.Vat,
                    i.VatDescription)).ToArray();

            var invoicePricesByVat = model.PricesByVat.Select(p => new RegisterOutgoingCreditNoteCommand.PriceByVat(
                -p.TaxableAmount,
                p.VatRate,
                -p.VatAmount,
                -p.TotalPrice,
                p.ProvidenceFundAmount == null ? null : -p.ProvidenceFundAmount)).ToArray();

            var nonTaxableItems = model.NonTaxableItems.Select(t => new RegisterOutgoingCreditNoteCommand.NonTaxableItem(t.Description, -t.Amount)).ToArray();

            var command = new RegisterOutgoingCreditNoteCommand(
                userId,
                model.Number,
                model.Date,
                model.Currency,
                -model.Amount,
                -model.Taxes,
                -model.TotalPrice,
                -model.TotalToPay,
                model.Description,
                model.PaymentTerms,
                model.PurchaseOrderNumber,
                model.Customer.OriginalId,
                model.Customer.Name,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                Settings.TaxId,
                Settings.CompanyName,
                Settings.Address,
                Settings.City,
                Settings.PostalCode,
                Settings.Country,
                Settings.TaxId,
                string.Empty,
                invoiceLineItems,
                model.VatIncluded,
                invoicePricesByVat,
                nonTaxableItems,
                model.ProvidenceFund?.Description,
                model.ProvidenceFund?.Rate,
                model.ProvidenceFund == null ? null : -model.ProvidenceFund?.Amount,
                model.WithholdingTax?.Description,
                model.WithholdingTax?.Rate,
                model.WithholdingTax?.TaxableAmountRate,
                model.WithholdingTax == null ? null : -model.WithholdingTax?.Amount);

            return command;
        }
        #endregion
    }
}
