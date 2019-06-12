using Merp.Accountancy.Settings;
using Merp.Accountancy.Settings.Commands;
using Merp.Accountancy.Web.Api.Internal.Models.Vat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Accountancy.Web.Api.Internal.WorkerServices
{
    public class VatControllerWorkerServices
    {
        public IDatabase Database { get; private set; }

        public VatCommands Commands { get; private set; }

        public VatControllerWorkerServices(IDatabase database, VatCommands commands)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public ListModel GetVatList(string filter, int page, int size)
        {
            string country = "IT";
            //TODO subscription id will be retrieve from user infos
            var subscriptionId = Guid.NewGuid();

            var vats = Database.Vats
                .NotUnlisted()
                .ByCountry(country)
                .SystemAndBySubscriptionId(subscriptionId);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                vats = vats.Where(v => v.Rate.ToString() == filter || v.Description.Contains(filter));
            }

            var vatList = vats
                .OrderBy(v => v.Rate)
                .Select(v => new ListModel.VatItem
                {
                    Id = v.Id,
                    Rate = v.Rate,
                    Description = v.Description,
                    IsSystemVat = v.SubscriptionId == Guid.Empty
                });

            int totalNumberOfVats = vatList.Count();

            int skip = (page - 1) * size;

            var model = new ListModel
            {
                TotalNumberOfVats = totalNumberOfVats,
                Vats = vatList.Skip(skip).Take(size).ToArray()
            };

            return model;
        }

        public IEnumerable<AvailableVatModel> GetAvailableVats(string query)
        {
            string country = "IT";

            var vats = Database.Vats
                .NotUnlisted()
                .ByCountry(country);

            if (!string.IsNullOrWhiteSpace(query))
            {
                vats = vats.Where(v => v.Rate.ToString() == query || v.Description.Contains(query));
            }

            return vats
                .OrderBy(v => v.Rate)
                .Select(v => new AvailableVatModel
                {
                    Id = v.Id,
                    Rate = v.Rate,
                    Description = v.Description,
                    AppliedForMinimumTaxPayer = v.AppliedForMinimumTaxPayer
                }).ToArray();
        }

        public async Task CreateNewVat(CreateModel model)
        {
            string country = "IT";

            //TODO subscription id will be loaded from current user infos
            var subscriptionId = Guid.NewGuid();
            await Commands.CreateVat(subscriptionId, country, model.Rate, model.Description);
        }

        public async Task EditVat(Guid vatId, EditModel model)
        {
            await Commands.EditVat(vatId, model.Rate, model.Description);
        }

        public async Task UnlistVat(Guid vatId)
        {
            await Commands.UnlistVat(vatId);
        }
    }
}
