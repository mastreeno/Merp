using Merp.Registry.Web.Api.Internal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class RegisterPartyDialog
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; } = default!;

        [Inject]
        public IPartyApiServices PartyApi { get; set; } = default!;

        private ViewModel model = new();

        private async Task RegisterPartyAsync()
        {
            var registerPartyModel = new Registry.Web.Api.Internal.Models.RegisterPartyModel
            {
                LastNameOrCompanyName = model.LastNameOrCompanyName,
                FirstName = model.FirstName,
                NationalIdentificationNumber = model.NationalIdentificationNumber,
                VatNumber = model.VatNumber,
                Address = new Registry.Web.Api.Internal.Models.PostalAddress
                {
                    Address = model.Address.Address,
                    City = model.Address.City,
                    Country = model.Address.Country,
                    PostalCode = model.Address.PostalCode,
                    Province = model.Address.Province
                }
            };
            await PartyApi.RegisterPartyAsync(registerPartyModel);

            var partyName = string.IsNullOrWhiteSpace(model.FirstName) ? model.LastNameOrCompanyName : $"{model.FirstName} {model.LastNameOrCompanyName}".Trim();
            Dialog.Close(DialogResult.Ok(new PartyRegistered(partyName)));
        }

        private void Close() => Dialog.Close(DialogResult.Cancel());

        public class ViewModel
        {
            [Required]
            public string LastNameOrCompanyName { get; set; } = string.Empty;

            public string FirstName { get; set; } = string.Empty;

            public string NationalIdentificationNumber { get; set; } = string.Empty;

            public string VatNumber { get; set; } = string.Empty;

            public PostalAddress Address { get; set; } = new();

            public record PostalAddress
            {
                [RequiredIfNotEmpty(nameof(City), nameof(PostalCode), nameof(Province), nameof(Country))]
                public string Address { get; set; } = string.Empty;

                [RequiredIfNotEmpty(nameof(Address), nameof(PostalCode), nameof(Province), nameof(Country))]
                public string City { get; set; } = string.Empty;

                public string PostalCode { get; set; } = string.Empty;

                public string Province { get; set; } = string.Empty;

                public string Country { get; set; } = string.Empty;
            }
        }

        public record PartyRegistered(string PartyName);
    }
}
