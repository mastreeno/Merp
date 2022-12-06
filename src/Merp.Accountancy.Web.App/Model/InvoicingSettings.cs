namespace Merp.Accountancy.Web.App.Model
{
    public record InvoicingSettings
    {
        public Guid PartyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string NationalIdentificationNumber { get; set; } = string.Empty;
    }
}
