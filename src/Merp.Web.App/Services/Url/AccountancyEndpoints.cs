using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Web.App.Services.Url
{
    public class AccountancyEndpoints : UrlBuilder.Endpoints
    {
        private readonly string _baseUrl;

        public string HomePageLocalization { get; private set; }

        public string GetInvoicesStats { get; private set; }

        public string JobOrderSearchLocalization { get; private set; }

        public string JobOrderCreateLocalization { get; private set; }

        public string JobOrderDetailLocalization { get; private set; }

        public string SearchJobOrders { get; private set; }

        public string GetJobOrderCustomers { get; private set; }

        public string CreateNewJobOrder { get; private set; }

        public string JobOrderDetail { get; private set; }

        public string GetJobOrderBalance { get; private set; }

        public string ExtendJobOrder { get; private set; }

        public string MarkJobOrderAsCompleted { get; private set; }

        public string GetOutgoingCreditNotesAssociatedToJobOrder { get; private set; }

        public string GetOutgoingInvoicesAssociatedToJobOrder { get; private set; }

        public string GetIncomingCreditNotesAssociatedToJobOrder { get; private set; }

        public string GetIncomingInvoicesAssociatedToJobOrder { get; private set; }

        public string InvoiceSearchLocalization { get; private set; }

        public string IssueOutgoingInvoiceLocalization { get; private set; }

        public string RegisterIncomingInvoiceLocalization { get; private set; }

        public string RegisterOutgoingInvoiceLocalization { get; private set; }

        public string OutgoingInvoicesNotLinkedToAJobOrderLocalization { get; private set; }

        public string IncomingInvoicesNotLinkedToAJobOrderLocalization { get; private set; }

        public string OutgoingCreditNoteDetailsLocalization { get; private set; }

        public string OutgoingInvoiceDetailsLocalization { get; private set; }

        public string IncomingCreditNoteDetailsLocalization { get; private set; }

        public string IncomingInvoiceDetailsLocalization { get; private set; }

        public string SearchInvoices { get; private set; }

        public string GetSearchInvoiceKinds { get; private set; }

        public string GetSearchInvoiceStates { get; private set; }

        public string GetInvoiceCustomers { get; private set; }

        public string GetInvoiceSuppliers { get; private set; }

        public string GetOutgoingDocumentTypes { get; private set; }

        public string GetIncomingDocumentTypes { get; private set; }

        public string IssueInvoice { get; private set; }

        public string IssueOutgoingCreditNoteFromDraft { get; private set; }

        public string IssueOutgoingInvoiceFromDraft { get; private set; }

        public string RegisterInvoice { get; private set; }

        public string RegisterOutgoingInvoice { get; private set; }

        public string GetOutgoingInvoicesNotLinkedToAJobOrder { get; private set; }

        public string LinkOutgoingInvoiceToJobOrder { get; private set; }

        public string GetIncomingInvoicesNotLinkedToAJobOrder { get; private set; }

        public string LinkIncomingInvoiceToJobOrder { get; private set; }

        public string OutgoingCreditNoteDetails { get; private set; }

        public string OutgoingInvoiceDetails { get; private set; }

        public string IncomingCreditNoteDetails { get; private set; }

        public string IncomingInvoiceDetails { get; private set; }

        public string GetInvoiceVatList { get; private set; }

        public string CreateOutgoingDraft { get; private set; }

        public string DeleteOutgoingCreditNoteDraft { get; private set; }

        public string DeleteOutgoingInvoiceDraft { get; private set; }

        public string EditOutgoingCreditNoteDraft { get; private set; }

        public string EditOutgoingCreditNoteDraftLocalization { get; set; }

        public string EditOutgoingInvoiceDraft { get; private set; }

        public string EditOutgoingInvoiceDraftLocalization { get; set; }

        public string GetDraftCustomers { get; private set; }

        public string SearchDrafts { get; private set; }

        public string SearchDraftsLocalization { get; private set; }

        public AccountancyEndpoints(string accountancyBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(accountancyBaseUrl))
            {
                throw new ArgumentException("value cannot be empty", nameof(accountancyBaseUrl));
            }

            _baseUrl = accountancyBaseUrl;

            HomePageLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Home/Index";

            JobOrderSearchLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/JobOrder/Search";
            JobOrderCreateLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/JobOrder/Create";
            JobOrderDetailLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/JobOrder/Detail";
            SearchJobOrders = $"{_baseUrl}{ApiPrefix}/JobOrder/Search";
            GetJobOrderCustomers = $"{_baseUrl}{ApiPrefix}/JobOrder/GetJobOrderCustomers";
            CreateNewJobOrder = $"{_baseUrl}{ApiPrefix}/JobOrder/Create";
            JobOrderDetail = $"{_baseUrl}{ApiPrefix}/JobOrder/Detail";
            GetJobOrderBalance = $"{_baseUrl}{ApiPrefix}/JobOrder/GetBalance";
            ExtendJobOrder = $"{_baseUrl}{ApiPrefix}/JobOrder/Extend";
            MarkJobOrderAsCompleted = $"{_baseUrl}{ApiPrefix}/JobOrder/MarkAsCompleted";
            GetOutgoingCreditNotesAssociatedToJobOrder = $"{_baseUrl}{ApiPrefix}/JobOrder/GetOutgoingCreditNotesAssociatedToJobOrder";
            GetOutgoingInvoicesAssociatedToJobOrder = $"{_baseUrl}{ApiPrefix}/JobOrder/GetOutgoingInvoicesAssociatedToJobOrder";
            GetIncomingCreditNotesAssociatedToJobOrder = $"{_baseUrl}{ApiPrefix}/JobOrder/GetIncomingCreditNotesAssociatedToJobOrder";
            GetIncomingInvoicesAssociatedToJobOrder = $"{_baseUrl}{ApiPrefix}/JobOrder/GetIncomingInvoicesAssociatedToJobOrder";

            InvoiceSearchLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/Search";
            IssueOutgoingInvoiceLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/IssueOutgoingInvoice";
            RegisterIncomingInvoiceLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/RegisterIncomingInvoice";
            RegisterOutgoingInvoiceLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/RegisterOutgoingInvoice";
            OutgoingInvoicesNotLinkedToAJobOrderLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/OutgoingInvoicesNotLinkedToAJobOrder";
            IncomingInvoicesNotLinkedToAJobOrderLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/IncomingInvoicesNotLinkedToAJobOrder";
            OutgoingCreditNoteDetailsLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/OutgoingCreditNoteDetails";
            OutgoingInvoiceDetailsLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/OutgoingInvoiceDetails";
            IncomingCreditNoteDetailsLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/IncomingCreditNoteDetails";
            IncomingInvoiceDetailsLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Invoice/IncomingInvoiceDetails";
            SearchInvoices = $"{_baseUrl}{ApiPrefix}/Invoice/Search";
            GetSearchInvoiceKinds = $"{_baseUrl}{ApiPrefix}/Invoice/GetSearchInvoiceKinds";
            GetSearchInvoiceStates = $"{_baseUrl}{ApiPrefix}/Invoice/GetSearchInvoiceStates";
            GetInvoiceCustomers = $"{_baseUrl}{ApiPrefix}/Invoice/GetInvoiceCustomers";
            GetInvoiceSuppliers = $"{_baseUrl}{ApiPrefix}/Invoice/GetInvoiceSuppliers";
            GetOutgoingDocumentTypes = $"{_baseUrl}{ApiPrefix}/Invoice/GetOutgoingDocumentTypes";
            GetIncomingDocumentTypes = $"{_baseUrl}{ApiPrefix}/Invoice/GetIncomingDocumentTypes";
            IssueInvoice = $"{_baseUrl}{ApiPrefix}/Invoice/IssueOutgoingInvoice";
            IssueOutgoingCreditNoteFromDraft = $"{_baseUrl}{ApiPrefix}/Invoice/IssueOutgoingCreditNoteFromDraft";
            IssueOutgoingInvoiceFromDraft = $"{_baseUrl}{ApiPrefix}/Invoice/IssueOutgoingInvoiceFromDraft";
            RegisterInvoice = $"{_baseUrl}{ApiPrefix}/Invoice/RegisterIncomingInvoice";
            RegisterOutgoingInvoice = $"{_baseUrl}{ApiPrefix}/Invoice/RegisterOutgoingInvoice";
            GetOutgoingInvoicesNotLinkedToAJobOrder = $"{_baseUrl}{ApiPrefix}/Invoice/OutgoingInvoicesNotLinkedToAJobOrder";
            LinkOutgoingInvoiceToJobOrder = $"{_baseUrl}{ApiPrefix}/Invoice/LinkOutgoingInvoiceToJobOrder";
            GetIncomingInvoicesNotLinkedToAJobOrder = $"{_baseUrl}{ApiPrefix}/Invoice/IncomingInvoicesNotLinkedToAJobOrder";
            LinkIncomingInvoiceToJobOrder = $"{_baseUrl}{ApiPrefix}/Invoice/LinkIncomingInvoiceToJobOrder";
            GetInvoicesStats = $"{_baseUrl}{ApiPrefix}/Invoice/GetInvoicesStats";
            OutgoingCreditNoteDetails = $"{_baseUrl}{ApiPrefix}/Invoice/OutgoingCreditNoteDetails";
            OutgoingInvoiceDetails = $"{_baseUrl}{ApiPrefix}/Invoice/OutgoingInvoiceDetails";
            IncomingCreditNoteDetails = $"{_baseUrl}{ApiPrefix}/Invoice/IncomingCreditNoteDetails";
            IncomingInvoiceDetails = $"{_baseUrl}{ApiPrefix}/Invoice/IncomingInvoiceDetails";
            GetInvoiceVatList = $"{_baseUrl}{ApiPrefix}/Invoice/GetVatList";

            CreateOutgoingDraft = $"{_baseUrl}{ApiPrefix}/Draft/CreateOutgoingDraft";
            DeleteOutgoingCreditNoteDraft = $"{_baseUrl}{ApiPrefix}/Draft/DeleteOutgoingCreditNoteDraft";
            DeleteOutgoingInvoiceDraft = $"{_baseUrl}{ApiPrefix}/Draft/DeleteOutgoingInvoiceDraft";
            EditOutgoingCreditNoteDraft = $"{_baseUrl}{ApiPrefix}/Draft/EditOutgoingCreditNoteDraft";
            EditOutgoingCreditNoteDraftLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Draft/EditOutgoingCreditNoteDraft";
            EditOutgoingInvoiceDraft = $"{_baseUrl}{ApiPrefix}/Draft/EditOutgoingInvoiceDraft";
            EditOutgoingInvoiceDraftLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Draft/EditOutgoingInvoiceDraft";
            GetDraftCustomers = $"{_baseUrl}{ApiPrefix}/Draft/GetDraftCustomers";
            SearchDrafts = $"{_baseUrl}{ApiPrefix}/Draft/Search";
            SearchDraftsLocalization = $"{_baseUrl}{ApiPrefix}{LocalizationApiPrefix}/Draft/Search";
        }
    }
}
