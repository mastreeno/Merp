using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Merp.Registry.QueryStack;
using Merp.Registry.Web.App.Model;

namespace Merp.Registry.Web.App.Pages
{
    public partial class AddCompany
    {
        [Inject] IDatabase Database { get; set; }

        public CompanyModel ViewModel = new();

        private void VatNumberLookup(VatNumber.PartyInfo partyInfo)
        {
            ViewModel.CompanyName = partyInfo.CompanyName;
            ViewModel.LegalAddress.Address = partyInfo.Address;
            ViewModel.LegalAddress.City = partyInfo.City;
            ViewModel.LegalAddress.Country = partyInfo.Country;
            ViewModel.LegalAddress.PostalCode = partyInfo.PostalCode;
            ViewModel.LegalAddress.Province = partyInfo.Province;
        }

        private async Task Submit()
        { 
        
        }

        public class CompanyModel
        {
            [Required]
            public string CompanyName { get; set; }
            public string NationalIdentificationNumber { get; set; }
            public string VatNumber { get; set; }

            public PostalAddress LegalAddress { get; set; } = new Model.PostalAddress();
            public bool UseLegalAddressAsBillingAddress { get; set; } = true;
            public PostalAddress BillingAddress { get; set; } = new Model.PostalAddress();
            public bool UseLegalAddressAsShippingAddress { get; set; } = true;
            public PostalAddress ShippingAddress { get; set; } = new Model.PostalAddress();

            [Phone]
            public string PhoneNumber { get; set; }

            [Phone]
            public string FaxNumber { get; set; }
            public string WebSiteUrl { get; set; }
            [EmailAddress]
            public string EmailAddress { get; set; }
        }
    }
}
