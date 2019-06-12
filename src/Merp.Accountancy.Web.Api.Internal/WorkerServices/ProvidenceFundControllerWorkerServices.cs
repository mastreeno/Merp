using Merp.Accountancy.Settings;
using Merp.Accountancy.Web.Api.Internal.Models.ProvidenceFund;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Merp.Accountancy.Web.Api.Internal.WorkerServices
{
    public class ProvidenceFundControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public ProvidenceFundControllerWorkerServices(IDatabase database)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IEnumerable<ListModel> GetProvidenceFunds(string query)
        {
            string country = "IT";
            var providenceFunds = Database.ProvidenceFunds
                .ByCountry(country);

            if (!string.IsNullOrWhiteSpace(query))
            {
                providenceFunds = providenceFunds.Where(p => p.Description.Contains(query));
            }

            return providenceFunds
                .OrderBy(p => p.Rate)
                .Select(p => new ListModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Rate = p.Rate,
                    AppliedInWithholdingTax = p.AppliedInWithholdingTax
                }).ToArray();
        }
    }
}
