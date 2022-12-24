using Merp.Registry.Web.Api.Internal;
using Merp.Registry.Web.Api.Internal.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class RegisterPersonDialog
    {
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; } = default!;

        [Inject]
        public IPersonApiServices PersonApi { get; set; } = default!;

        private ViewModel model = new();

        private async Task RegisterPersonAsync()
        {
            var registerPersonModel = new RegisterPersonModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                NationalIdentificationNumber = model.NationalIdentificationNumber,
                VatNumber = model.VatNumber,
                Address = new PostalAddress
                {
                    Address = model.Address.Address,
                    City = model.Address.City,
                    Country = model.Address.Country,
                    PostalCode = model.Address.PostalCode,
                    Province = model.Address.Province
                }
            };

            await PersonApi.RegisterPersonAsync(registerPersonModel);
            Dialog.Close(DialogResult.Ok(new PersonRegistered(model.FirstName, model.LastName)));
        }

        private void Close() => Dialog.Close(DialogResult.Cancel());

        public class ViewModel
        {
            [Required]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            public string LastName { get; set; } = string.Empty;

            [StringLength(16, MinimumLength = 16)]
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

        public record PersonRegistered(string FirstName, string LastName);
    }
}
