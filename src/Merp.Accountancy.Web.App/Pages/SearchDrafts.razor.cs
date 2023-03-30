using Merp.Accountancy.Drafts;
using Merp.Accountancy.Drafts.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class SearchDrafts
    {
        [Inject] public IDatabase Database { get; set; } = default!;

        private SearchParameters parameters = new();

        private SearchResult model = new();

        private int numberOfPages = 0;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadDraftCustomersAsync();
        }

        private async Task LoadDraftCustomersAsync()
        {
            var customers = new List<SearchParameters.CustomerDescriptor>();

            var invoiceDraftCustomers = await Database.OutgoingInvoiceDrafts
                .OrderBy(i => i.Customer.Name)
                .Select(i => new SearchParameters.CustomerDescriptor { Id = i.Customer.OriginalId, Name = i.Customer.Name })
                .Distinct()
                .ToArrayAsync();

            if (invoiceDraftCustomers.Any())
            {
                customers.AddRange(invoiceDraftCustomers);
            }

            var creditNoteDraftCustomers = await Database.OutgoingCreditNoteDrafts
                .OrderBy(i => i.Customer.Name)
                .Select(i => new SearchParameters.CustomerDescriptor { Id = i.Customer.OriginalId, Name = i.Customer.Name })
                .Distinct()
                .ToArrayAsync();

            if (creditNoteDraftCustomers.Any())
            {
                customers.AddRange(creditNoteDraftCustomers);
            }

            parameters.DraftCustomers = customers.OrderBy(c => c.Name).Distinct();
        }

        private async Task SearchDraftsAsync()
        {
            var drafts = parameters.Kind switch
            {
                DraftKind.OutgoingInvoices => SearchOutgoingInvoiceDrafts(parameters),
                DraftKind.OutgoingCreditNotes => SearchOutgoingCreditNoteDrafts(parameters),
                _ => SearchAllDrafts(parameters)
            };

            var totalNumberOfDrafts = drafts.Count();

            int skip = (parameters.PageIndex - 1) * parameters.PageSize;
            model.TotalNumberOfDrafts = totalNumberOfDrafts;
            model.Drafts = await drafts.OrderByDescending(i => i.Date).Skip(skip).Take(parameters.PageSize).ToListAsync();

            numberOfPages = (int)Math.Ceiling(totalNumberOfDrafts / (decimal)parameters.PageSize);
        }

        private async Task ClearSearchParametersAsync()
        {
            parameters = new();
            await LoadDraftCustomersAsync();

            await SearchDraftsAsync();
        }

        private async Task OnPageChanged(int pageIndex)
        {
            parameters.PageIndex = pageIndex;
            await SearchDraftsAsync();
        }

        private string BuildDraftEditUrl(SearchResult.DraftDescriptor draft)
        {
            return draft.DocumentType switch
            {
                DraftKind.OutgoingInvoices => UrlBuilder.BuildEditOutgoingInvoiceDraftUrl(draft.Id),
                DraftKind.OutgoingCreditNotes => UrlBuilder.BuildEditOutgoingCreditNoteDraftUrl(draft.Id),
                _ => throw new InvalidOperationException("Invalid document type")
            };
        }

        #region Draft search by kind
        private IQueryable<SearchResult.DraftDescriptor> SearchAllDrafts(SearchParameters parameters)
        {
            var outgoingInvoiceDrafts = SearchOutgoingInvoiceDrafts(parameters);
            var outgoingCreditNoteDrafts = SearchOutgoingCreditNoteDrafts(parameters);

            return outgoingInvoiceDrafts.Concat(outgoingCreditNoteDrafts);
        }

        private IQueryable<SearchResult.DraftDescriptor> SearchOutgoingInvoiceDrafts(SearchParameters parameters)
        {
            var outgoingInvoiceDrafts = Database.OutgoingInvoiceDrafts;
            outgoingInvoiceDrafts = FilterByCustomerId(outgoingInvoiceDrafts, parameters.CustomerId);
            outgoingInvoiceDrafts = FilterByDate(outgoingInvoiceDrafts, parameters.DateFrom, parameters.DateTo);

            return outgoingInvoiceDrafts.Select(d => new SearchResult.DraftDescriptor
            {
                Id = d.Id,
                CustomerName = d.Customer.Name,
                Date = d.Date,
                DocumentType = DraftKind.OutgoingInvoices,
                TotalPrice = d.TotalPrice,
                Currency = d.Currency
            });
        }

        private IQueryable<SearchResult.DraftDescriptor> SearchOutgoingCreditNoteDrafts(SearchParameters parameters)
        {
            var outgoingCreditNoteDrafts = Database.OutgoingCreditNoteDrafts;
            outgoingCreditNoteDrafts = FilterByCustomerId(outgoingCreditNoteDrafts, parameters.CustomerId);
            outgoingCreditNoteDrafts = FilterByDate(outgoingCreditNoteDrafts, parameters.DateFrom, parameters.DateTo);

            return outgoingCreditNoteDrafts.Select(d => new SearchResult.DraftDescriptor
            {
                Id = d.Id,
                CustomerName = d.Customer.Name,
                Date = d.Date,
                DocumentType = DraftKind.OutgoingCreditNotes,
                TotalPrice = d.TotalPrice,
                Currency = d.Currency
            });
        }

        private IQueryable<TDraft> FilterByCustomerId<TDraft>(IQueryable<TDraft> drafts, Guid? customerId)
            where TDraft : InvoiceDraft
        {
            if (customerId.HasValue)
            {
                drafts = drafts.Where(d => d.Customer.OriginalId == customerId);
            }

            return drafts;
        }

        private IQueryable<TDraft> FilterByDate<TDraft>(IQueryable<TDraft> drafts, DateTime? dateFrom, DateTime? dateTo)
            where TDraft : InvoiceDraft
        {
            if (dateFrom.HasValue)
            {
                drafts = drafts.Where(i => i.Date >= dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                drafts = drafts.Where(i => i.Date <= dateTo.Value);
            }

            return drafts;
        }
        #endregion

        public enum DraftKind
        {
            Any,
            OutgoingInvoices,
            OutgoingCreditNotes
        }

        public class SearchResult
        {
            public int TotalNumberOfDrafts { get; set; }

            public IEnumerable<DraftDescriptor> Drafts { get; set; } = Enumerable.Empty<DraftDescriptor>();

            public class DraftDescriptor
            {
                public Guid Id { get; set; }

                public DraftKind DocumentType { get; set; }

                public DateTime? Date { get; set; }

                public string CustomerName { get; set; } = string.Empty;

                public decimal TotalPrice { get; set; }

                public string Currency { get; set; } = string.Empty;
            }
        }

        public class SearchParameters
        {
            public DraftKind Kind { get; set; } = DraftKind.Any;

            public int PageIndex { get; set; } = 1;

            public int PageSize { get; set; } = 20;

            public Guid? CustomerId { get; set; }

            public DateTime? DateFrom { get; set; }

            public DateTime? DateTo { get; set; }

            public IEnumerable<CustomerDescriptor> DraftCustomers { get; set; } = Enumerable.Empty<CustomerDescriptor>();

            public record CustomerDescriptor
            {
                public Guid Id { get; set; }

                public string Name { get; set; } = string.Empty;
            }
        }
    }
}
