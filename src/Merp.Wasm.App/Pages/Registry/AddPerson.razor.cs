using Merp.Wasm.App.Http;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Wasm.App.Pages.Registry
{
    public partial class AddPerson
    {
        [Inject] RegistryPrivateApiHttpClient Http { get; set; }
        public ViewModel Model = new();

        private async Task Submit()
        {

        }

        public class ViewModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            public string NationalIdentificationNumber { get; set; }
            public string VatNumber { get; set; }

            public bool UseLegalAddressAsBillingAddress { get; set; } = true;
            public Merp.Wasm.App.Model.PostalAddress BillingAddress { get; set; } = new Model.PostalAddress();
            public Merp.Wasm.App.Model.PostalAddress LegalAddress { get; set; } = new Model.PostalAddress();
            public bool UseLegalAddressAsShippingAddress { get; set; } = true;
            public Merp.Wasm.App.Model.PostalAddress ShippingAddress { get; set; } = new Model.PostalAddress();

            [Phone]
            public string PhoneNumber { get; set; }
            [Phone]
            public string MobilePhoneNumber { get; set; }
            [Phone]
            public string FaxNumber { get; set; }
            public string WebSiteUrl { get; set; }
            [EmailAddress]
            public string EmailAddress { get; set; }
            public string InstantMessaging { get; set; }
        }
    }
}
