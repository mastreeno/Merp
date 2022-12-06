using Merp.Accountancy.Web.App.Model;
using Microsoft.Extensions.Configuration;

namespace Merp.Accountancy.Web.App.Services
{
    public interface IAccountancySettingsProvider
    {
        IEnumerable<Vat> GetVatList();

        InvoicingSettings GetInvoicingSettings();
    }

    #region Mock Accountancy settings provider
    public class MockAccountancySettingsProvider : IAccountancySettingsProvider
    {
        private static readonly Guid _invoicingSettingsPartyId = Guid.NewGuid();

        private readonly IConfiguration _configuration;

        public MockAccountancySettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public InvoicingSettings GetInvoicingSettings()
        {
            var invoicingSettings = new InvoicingSettings { PartyId = _invoicingSettingsPartyId };
            _configuration.GetSection("Modules:Accountancy:Settings:InvoicingSettings").Bind(invoicingSettings);

            return invoicingSettings;
        }

        public IEnumerable<Vat> GetVatList()
        {
            var vatList = new List<Vat>();
            _configuration.GetSection("Modules:Accountancy:Settings:VatList").Bind(vatList);

            return vatList;
        }
    }
    #endregion
}
