using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Acl.RegistryResolutionServices;

namespace Merp.Web.App.Pages.Registry
{
    public partial class VatNumber
    {
        [Inject] Resolver ViesAclProxy { get; set; }
        private string vatIndex;
        private string countryCode;

        private IEnumerable<Country> countries = new List<Country>();

        [Parameter]
        public EventCallback<PartyInfo> OnLookup { get; set; }

        [Parameter]
        public PartyType Type { get; set; }

        [Parameter]
        public string Value
        {
            set
            {
                if (value != null && value.Length > 3 && countries.Any(c => c.Code == value.Substring(0, 2)))
                {
                    countryCode = value.Substring(0, 2);
                    vatIndex = value.Substring(2);
                }
                else
                {
                    vatIndex = value;
                }
            }
            get
            {
                return vatIndex switch
                {
                    null => null,
                    "" => null,
                    string vi when vi.Length == 0 => null,
                    string vi when vi.Length > 0 => string.Concat(countryCode, vi),
                    _ => vatIndex
                };
            }
        }

        protected override async Task OnInitializedAsync()
        {           

        }

        private async void Lookup()
        {
            if (string.IsNullOrWhiteSpace(this.countryCode) || string.IsNullOrWhiteSpace(this.vatIndex))
                return;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CompanyInformation, PartyInfo>();
                cfg.CreateMap<PersonInformation, PartyInfo>();
            });

            PartyInfo partyInfo = null;
            IMapper mapper = config.CreateMapper();
            switch(Type)
            {
                case PartyType.Company:
                    var companyInfo = await ViesAclProxy.LookupCompanyInfoByVatNumberAsync(countryCode, vatIndex);
                    partyInfo = mapper.Map<CompanyInformation, PartyInfo>(companyInfo);
                    break;
                case PartyType.Person:
                    var personInfo = await ViesAclProxy.LookupPersonInfoByVatNumberAsync(countryCode, vatIndex);
                    partyInfo = mapper.Map<PersonInformation, PartyInfo>(personInfo);
                    break;
            }
            if (partyInfo != null)
                await OnLookup.InvokeAsync(partyInfo);

            Console.WriteLine(partyInfo?.FirstName);
        }

        private IEnumerable<Country> MockGetCountries()
        {
            return new List<Country>()
            {
                new Country { Code="AT", DisplayName = "Austria" },
                new Country { Code="BE", DisplayName = "Belgium" },
                new Country { Code="BG", DisplayName = "Bulgaria" },
                new Country { Code="CY", DisplayName = "Cyprus" },
                new Country { Code="CZ", DisplayName = "Czech Republic" },
                new Country { Code="DE", DisplayName = "Germany" },
                new Country { Code="DK", DisplayName = "Denmark" },
                new Country { Code="EE", DisplayName = "Estonia" },
                new Country { Code="EL", DisplayName = "Greece" },
                new Country { Code="ES", DisplayName = "Spain" },
                new Country { Code="FI", DisplayName = "Finland" },
                new Country { Code="FR", DisplayName = "France" },
                new Country { Code="GB", DisplayName = "United Kingdom" },
                new Country { Code="HR", DisplayName = "Croatia" },
                new Country { Code="HU", DisplayName = "Hungary" },
                new Country { Code="IE", DisplayName = "Ireland" },
                new Country { Code="IT", DisplayName = "Italy" },
                new Country { Code="LT", DisplayName = "Lithuania" },
                new Country { Code="LU", DisplayName = "Luxemburg" },
                new Country { Code="LV", DisplayName = "Latvia" },
                new Country { Code="MT", DisplayName = "Malta" },
                new Country { Code="NL", DisplayName = "Netherlands" },
                new Country { Code="PL", DisplayName = "Poland" },
                new Country { Code="PT", DisplayName = "Portugal" },
                new Country { Code="RO", DisplayName = "Romania" },
                new Country { Code="SE", DisplayName = "Sweden" },
                new Country { Code="SI", DisplayName = "Slovenia" },
                new Country { Code="SK", DisplayName = "Slovakia" },
            };
        }

        public enum PartyType
        {
            Company,
            Person
        }

        public class PartyInfo
        {
            public string CompanyName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string VatNumber { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
        }

        public class Country
        {
            public string Code { get; set; }
            public string DisplayName { get; set; }
        }

        public class GetCompanyInfoByVatNumberModel
        {
            public string CompanyName { get; set; }
            public string VatNumber { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
        }

        public class GetPersonInfoByVatNumberModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string VatNumber { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
        }
    }
}
