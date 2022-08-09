using Merp.Accountancy.QueryStack;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class SearchJobOrders
    {
        [Inject] public IDatabase Database { get; set; } = default!;

        private SearchParameters parameters = new();

        private SearchResult model = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var jobOrderCustomers = Database.JobOrders
                .OrderBy(j => j.CustomerName)
                .Select(j => new SearchParameters.CustomerDescriptor
                {
                    Id = j.CustomerId,
                    CustomerName = j.CustomerName
                }).Distinct();

            parameters.Customers = await jobOrderCustomers.ToArrayAsync();
        }

        async Task Search()
        {
            //TODO implementing the job order search
            var jobOrders = Database.JobOrders;

            if (parameters.CustomerId.HasValue)
            {
                jobOrders = jobOrders.Where(j => j.CustomerId == parameters.CustomerId.Value);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                jobOrders = jobOrders.Where(j => j.Name.Contains(parameters.Name));
            }
            if (parameters.CurrentOnly)
            {
                jobOrders = jobOrders.CurrentOnly();
            }

            int skip = (parameters.PageIndex - 1) * parameters.PageSize;

            var jobOrdersViewModel = jobOrders
                .OrderBy(j => j.CustomerName)
                .ThenBy(j => j.Name)
                .Select(j => new SearchResult.JobOrderDescriptor
                {
                    Id = j.OriginalId,
                    CustomerName = j.CustomerName,
                    Name = j.Name
                });

            var totalNumberOfJobOrders = jobOrdersViewModel.Count();

            model.TotalNumberOfJobOrders = totalNumberOfJobOrders;
            model.JobOrders = await jobOrdersViewModel.Skip(skip).Take(parameters.PageSize).ToArrayAsync();
        }

        public class SearchResult
        {
            public int TotalNumberOfJobOrders { get; set; }

            public IEnumerable<JobOrderDescriptor> JobOrders { get; set; } = Array.Empty<JobOrderDescriptor>();

            public class JobOrderDescriptor
            {
                public Guid Id { get; set; }

                public string CustomerName { get; set; } = string.Empty;

                public string Name { get; set; } = string.Empty;
            }
        }

        public class SearchParameters
        {
            public int PageIndex { get; set; } = 1;

            public int PageSize { get; set; } = 20;

            public Guid? CustomerId { get; set; }

            public string Name { get; set; } = string.Empty;

            public bool CurrentOnly { get; set; }

            public IEnumerable<CustomerDescriptor> Customers { get; set; } = Array.Empty<CustomerDescriptor>();

            public record CustomerDescriptor
            {
                public Guid Id { get; set; }

                public string CustomerName { get; set; } = string.Empty;
            }
        }
    }
}
