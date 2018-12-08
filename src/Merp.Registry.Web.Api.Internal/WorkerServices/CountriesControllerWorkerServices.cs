using Merp.Registry.Web.Api.Internal.Models.Countries;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Merp.Registry.Web.Api.Internal.WorkerServices
{
    public class CountriesControllerWorkerServices
    {
        private readonly string[] _countryCodes;

        public CountriesControllerWorkerServices()
        {
            _countryCodes = new string[]
            {
                "AT", "BE", "BG", "CY", "CZ", "DE", "DK", "EE", "EL", "ES", "FI", "FR", "GB", "HR", "HU", "IE", "IT", "LT", "LU", "LV", "MT", "NL", "PL", "PT", "RO", "SE", "SI", "SK"
            };
        }

        public IEnumerable<CountryModel> GetCountries(CultureInfo currentCulture)
        {
            var assembly = this.GetType().Assembly;
            var resourceManager = new ResourceManager($"{assembly.GetName().Name}.Resources.Countries", assembly);

            return _countryCodes.Select(c => new CountryModel
            {
                Code = c,
                DisplayName = $"{c}-{resourceManager.GetString(c, currentCulture)}"
            });
        }
    }
}
