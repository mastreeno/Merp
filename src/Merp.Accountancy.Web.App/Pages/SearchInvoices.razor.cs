using Merp.Accountancy.QueryStack;
using Merp.Accountancy.QueryStack.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class SearchInvoices
    {
        [Inject] public IDatabase Database { get; set; } = default!;

        private SearchParameters parameters = new();

        private SearchResult model = new();

        private int numberOfPages = 0;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadInvoiceCustomersAsync();
            await LoadInvoiceSuppliersAsync();
        }

        #region Customers and Suppliers loading
        private async Task LoadInvoiceSuppliersAsync()
        {
            var suppliers = new List<SearchParameters.PartyDescriptor>();

            var incomingInvoiceSuppliers = await Database.IncomingInvoices
                .OrderBy(i => i.Supplier.Name)
                .Select(i => new SearchParameters.PartyDescriptor { Id = i.Supplier.OriginalId, Name = i.Supplier.Name })
                .Distinct()
                .ToArrayAsync();

            if (incomingInvoiceSuppliers.Any())
            {
                suppliers.AddRange(incomingInvoiceSuppliers);
            }

            var incomingCreditNoteSuppliers = await Database.IncomingCreditNotes
                .OrderBy(c => c.Supplier.Name)
                .Select(c => new SearchParameters.PartyDescriptor { Id = c.Supplier.OriginalId, Name = c.Supplier.Name })
                .Distinct()
                .ToArrayAsync();

            if (incomingCreditNoteSuppliers.Any())
            {
                suppliers.AddRange(incomingCreditNoteSuppliers);
            }

            parameters.InvoiceSuppliers = suppliers.OrderBy(s => s.Name).Distinct();
        }

        private async Task LoadInvoiceCustomersAsync()
        {
            var customers = new List<SearchParameters.PartyDescriptor>();

            var outgoingInvoiceCustomers = await Database.OutgoingInvoices
                .OrderBy(i => i.Customer.Name)
                .Select(i => new SearchParameters.PartyDescriptor { Id = i.Customer.OriginalId, Name = i.Customer.Name })
                .Distinct()
                .ToArrayAsync();

            if (outgoingInvoiceCustomers.Any())
            {
                customers.AddRange(outgoingInvoiceCustomers);
            }

            var outgoingCreditNoteCustomers = await Database.OutgoingCreditNotes
                .OrderBy(c => c.Customer.Name)
                .Select(c => new SearchParameters.PartyDescriptor { Id = c.Customer.OriginalId, Name = c.Customer.Name })
                .Distinct()
                .ToArrayAsync();

            if (outgoingCreditNoteCustomers.Any())
            {
                customers.AddRange(outgoingCreditNoteCustomers);
            }

            parameters.InvoiceCustomers = customers.OrderBy(c => c.Name).Distinct();
        }
        #endregion

        private async Task SearchInvoicesAsync()
        {
            var invoices = parameters.Kind switch
            {
                InvoiceKind.OutgoingInvoices => SearchOutgoingInvoices(parameters),
                InvoiceKind.OutgoingCreditNotes => SearchOutgoingCreditNotes(parameters),
                InvoiceKind.IncomingInvoices => SearchIncomingInvoices(parameters),
                InvoiceKind.IncomingCreditNotes => SearchIncomingCreditNotes(parameters),
                _ => SearchAllInvoices(parameters)
            };

            var totalNumberOfInvoices = invoices.Count();

            int skip = (parameters.PageIndex - 1) * parameters.PageSize;
            model.TotalNumberOfInvoices = totalNumberOfInvoices;
            model.Invoices = await invoices.OrderByDescending(i => i.Date).Skip(skip).Take(parameters.PageSize).ToArrayAsync();

            numberOfPages = (int)Math.Ceiling(totalNumberOfInvoices / (decimal)parameters.PageSize);
        }

        private async Task ClearSearchParametersAsync()
        {
            parameters = new();
            await LoadInvoiceCustomersAsync();
            await LoadInvoiceSuppliersAsync();

            await SearchInvoicesAsync();
        }

        private async Task OnPageChanged(int pageIndex)
        {
            parameters.PageIndex = pageIndex;
            await SearchInvoicesAsync();
        }

        #region Invoice search by kind
        private IQueryable<SearchResult.InvoiceDescriptor> SearchOutgoingInvoices(SearchParameters parameters)
        {
            var outgoingInvoices = parameters.Status switch
            {
                SearchParameters.InvoiceState.Overdue => Database.OutgoingInvoices.Overdue(),
                SearchParameters.InvoiceState.Outstanding => Database.OutgoingInvoices.Outstanding(),
                SearchParameters.InvoiceState.Paid => Database.OutgoingInvoices.Where(i => i.IsPaid),
                _ => Database.OutgoingInvoices
            };

            outgoingInvoices = FilterInvoicesByDate(outgoingInvoices, parameters.DateFrom, parameters.DateTo);
            outgoingInvoices = FilterInvoicesByCustomer(outgoingInvoices, parameters.CustomerId);
            outgoingInvoices = FilterInvoicesBySupplier(outgoingInvoices, parameters.SupplierId);

            return outgoingInvoices
                .Select(i => new SearchResult.InvoiceDescriptor
                {
                    Id = i.OriginalId,
                    Currency = i.Currency,
                    CustomerName = i.Customer.Name,
                    Date = i.Date,
                    DocumentType = InvoiceKind.OutgoingInvoices,
                    DueDate = i.DueDate,
                    Number = i.Number,
                    SupplierName = i.Supplier.Name,
                    TotalPrice = i.TotalPrice
                });
        }

        private IQueryable<SearchResult.InvoiceDescriptor> SearchOutgoingCreditNotes(SearchParameters parameters)
        {
            var outgoingCreditNotes = parameters.Status switch
            {
                SearchParameters.InvoiceState.Overdue => Database.OutgoingCreditNotes.Overdue(),
                SearchParameters.InvoiceState.Outstanding => Database.OutgoingCreditNotes.Outstanding(),
                SearchParameters.InvoiceState.Paid => Database.OutgoingCreditNotes.Where(i => i.IsPaid),
                _ => Database.OutgoingCreditNotes
            };

            outgoingCreditNotes = FilterInvoicesByDate(outgoingCreditNotes, parameters.DateFrom, parameters.DateTo);
            outgoingCreditNotes = FilterInvoicesByCustomer(outgoingCreditNotes, parameters.CustomerId);
            outgoingCreditNotes = FilterInvoicesBySupplier(outgoingCreditNotes, parameters.SupplierId);

            return outgoingCreditNotes
                .Select(i => new SearchResult.InvoiceDescriptor
                {
                    Id = i.OriginalId,
                    Currency = i.Currency,
                    CustomerName = i.Customer.Name,
                    Date = i.Date,
                    DocumentType = InvoiceKind.OutgoingCreditNotes,
                    DueDate = i.DueDate,
                    Number = i.Number,
                    SupplierName = i.Supplier.Name,
                    TotalPrice = i.TotalPrice
                });
        }

        private IQueryable<SearchResult.InvoiceDescriptor> SearchIncomingInvoices(SearchParameters parameters)
        {
            var incomingInvoices = parameters.Status switch
            {
                SearchParameters.InvoiceState.Overdue => Database.IncomingInvoices.Overdue(),
                SearchParameters.InvoiceState.Outstanding => Database.IncomingInvoices.Outstanding(),
                SearchParameters.InvoiceState.Paid => Database.IncomingInvoices.Where(i => i.IsPaid),
                _ => Database.IncomingInvoices
            };

            incomingInvoices = FilterInvoicesByDate(incomingInvoices, parameters.DateFrom, parameters.DateTo);
            incomingInvoices = FilterInvoicesByCustomer(incomingInvoices, parameters.CustomerId);
            incomingInvoices = FilterInvoicesBySupplier(incomingInvoices, parameters.SupplierId);

            return incomingInvoices
                .Select(i => new SearchResult.InvoiceDescriptor
                {
                    Id = i.OriginalId,
                    Currency = i.Currency,
                    CustomerName = i.Customer.Name,
                    Date = i.Date,
                    DocumentType = InvoiceKind.IncomingInvoices,
                    DueDate = i.DueDate,
                    Number = i.Number,
                    SupplierName = i.Supplier.Name,
                    TotalPrice = i.TotalPrice
                });
        }

        private IQueryable<SearchResult.InvoiceDescriptor> SearchIncomingCreditNotes(SearchParameters parameters)
        {
            var incomingCreditNotes = parameters.Status switch
            {
                SearchParameters.InvoiceState.Overdue => Database.IncomingCreditNotes.Overdue(),
                SearchParameters.InvoiceState.Outstanding => Database.IncomingCreditNotes.Outstanding(),
                SearchParameters.InvoiceState.Paid => Database.IncomingCreditNotes.Where(i => i.IsPaid),
                _ => Database.IncomingCreditNotes
            };

            incomingCreditNotes = FilterInvoicesByDate(incomingCreditNotes, parameters.DateFrom, parameters.DateTo);
            incomingCreditNotes = FilterInvoicesByCustomer(incomingCreditNotes, parameters.CustomerId);
            incomingCreditNotes = FilterInvoicesBySupplier(incomingCreditNotes, parameters.SupplierId);

            return incomingCreditNotes
                .Select(i => new SearchResult.InvoiceDescriptor
                {
                    Id = i.OriginalId,
                    Currency = i.Currency,
                    CustomerName = i.Customer.Name,
                    Date = i.Date,
                    DocumentType = InvoiceKind.IncomingCreditNotes,
                    DueDate = i.DueDate,
                    Number = i.Number,
                    SupplierName = i.Supplier.Name,
                    TotalPrice = i.TotalPrice
                });
        }

        private IQueryable<SearchResult.InvoiceDescriptor> SearchAllInvoices(SearchParameters parameters)
        {
            var outgoingInvoices = SearchOutgoingInvoices(parameters);
            var outgoingCreditNotes = SearchOutgoingCreditNotes(parameters);
            var incomingInvoices = SearchIncomingInvoices(parameters);
            var incomingCreditNotes = SearchIncomingCreditNotes(parameters);

            return outgoingInvoices.Concat(outgoingCreditNotes).Concat(incomingInvoices).Concat(incomingCreditNotes);
        }

        private IQueryable<Invoice> FilterInvoicesByDate(IQueryable<Invoice> invoices, DateTime? dateFrom, DateTime? dateTo)
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

        private IQueryable<Invoice> FilterInvoicesBySupplier(IQueryable<Invoice> invoices, Guid? supplierId)
        {
            if (supplierId.HasValue)
            {
                invoices = invoices.Where(i => i.Supplier.OriginalId == supplierId.Value);
            }

            return invoices;
        }

        private IQueryable<Invoice> FilterInvoicesByCustomer(IQueryable<Invoice> invoices, Guid? customerId)
        {
            if (customerId.HasValue)
            {
                invoices = invoices.Where(i => i.Customer.OriginalId == customerId.Value);
            }

            return invoices;
        }
        #endregion

        public class SearchResult
        {
            public int TotalNumberOfInvoices { get; set; }

            public IEnumerable<InvoiceDescriptor> Invoices { get; set; } = Enumerable.Empty<InvoiceDescriptor>();

            public class InvoiceDescriptor
            {
                public Guid Id { get; set; }

                public InvoiceKind DocumentType { get; set; }

                public string Number { get; set; } = string.Empty;

                public DateTime Date { get; set; }

                public DateTime? DueDate { get; set; }

                public string SupplierName { get; set; } = string.Empty;

                public string CustomerName { get; set; } = string.Empty;

                public decimal TotalPrice { get; set; }

                public string Currency { get; set; } = string.Empty;
            }
        }

        public enum InvoiceKind
        {
            Any,
            IncomingInvoices,
            OutgoingInvoices,
            IncomingCreditNotes,
            OutgoingCreditNotes
        }

        public class SearchParameters
        {
            public int PageIndex { get; set; } = 1;

            public int PageSize { get; set; } = 20;

            public InvoiceKind Kind { get; set; } = InvoiceKind.Any;

            public InvoiceState Status { get; set; } = InvoiceState.Any;

            public IEnumerable<PartyDescriptor> InvoiceSuppliers { get; set; } = Enumerable.Empty<PartyDescriptor>();

            public Guid? SupplierId { get; set; }

            public IEnumerable<PartyDescriptor> InvoiceCustomers { get; set; } = Enumerable.Empty<PartyDescriptor>();

            public Guid? CustomerId { get; set; }

            public DateTime? DateFrom { get; set; }

            public DateTime? DateTo { get; set; }

            public enum InvoiceState
            {
                Any,
                Outstanding,
                Overdue,
                Paid
            }

            public record PartyDescriptor
            {
                public Guid Id { get; set; }

                public string Name { get; set; } = string.Empty;
            }
        }
    }
}
