using Merp.Accountancy.Settings;
using Merp.Accountancy.Web.Api.Internal.Models.WithholdingTax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Merp.Accountancy.Web.Api.Internal.WorkerServices
{
    public class WithholdingTaxControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public WithholdingTaxControllerWorkerServices(IDatabase database)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IEnumerable<ListModel> GetWithholdingTaxes(string query)
        {
            string country = "IT";
            var withholdingTaxes = Database.WithholdingTaxes
                .ByCountry(country);

            if (!string.IsNullOrWhiteSpace(query))
            {
                withholdingTaxes = withholdingTaxes.Where(w => w.Description.Contains(query));
            }

            return withholdingTaxes
                .OrderBy(w => w.Rate)
                .Select(w => new ListModel
                {
                    Id = w.Id,
                    Description = w.Description,
                    Rate = w.Rate,
                    TaxableAmountRate = w.TaxableAmountRate
                }).ToArray();
        }
    }
}
