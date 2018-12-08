using System;

namespace Merp.Accountancy.Web.Models
{
    public class BillingInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string VatNumber { get; set; }

        public string NationalIdentificationNumber { get; set; }
    }
}
