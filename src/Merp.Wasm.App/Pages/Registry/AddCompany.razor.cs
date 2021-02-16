using Merp.Wasm.App.Http;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Wasm.App.Pages.Registry
{
    public partial class AddCompany
    {
        [Inject] RegistryPrivateApiHttpClient Http { get; set; }
        public ViewModel Model = new();

        private async Task Submit()
        { 
        
        }

        public class ViewModel
        {
            [Required]
            public string CompanyName { get; set; }
            public string NationalIdentificationNumber { get; set; }
            public string VatNumber { get; set; }

            public Merp.Wasm.App.Model.PostalAddress BillingAddress { get; set; } = new Model.PostalAddress();
            public Merp.Wasm.App.Model.PostalAddress LegalAddress { get; set; } = new Model.PostalAddress();
            public Merp.Wasm.App.Model.PostalAddress ShippingAddress { get; set; } = new Model.PostalAddress();
        }
    }
}
