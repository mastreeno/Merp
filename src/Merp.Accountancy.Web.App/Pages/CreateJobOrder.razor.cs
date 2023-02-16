using Merp.Accountancy.CommandStack.Commands;
using Merp.Accountancy.Web.App.Components;
using Merp.Accountancy.Web.App.Model;
using Merp.Web;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class CreateJobOrder
    {
        [Inject] public IBus Bus { get; set; } = default!;

        [Inject] IAppContext AppContext { get; set; } = default!;

        private ViewModel model = new();

        private async Task SubmitAsync()
        {
            var command = new RegisterJobOrderCommand(
                AppContext.UserId,
                model.Customer.OriginalId,
                model.Customer.Name,
                model.ContactPerson?.OriginalId,
                model.Manager.OriginalId,
                model.Price.Amount,
                model.Price.Currency,
                model.DateOfStart!.Value,
                model.DueDate!.Value,
                model.IsTimeAndMaterial,
                model.Name,
                model.PurchaseOrderNumber,
                model.Description);

            await Bus.Send(command);

            NavigationManager.NavigateTo(UrlBuilder.BuildSearchJobOrdersUrl());
        }

        private void Cancel() => model = new();

        class ViewModel
        {
            public PartyInfoAutocomplete.ViewModel? Customer { get; set; }

            public PersonInfoAutocomplete.ViewModel? ContactPerson { get; set; }

            public PersonInfoAutocomplete.ViewModel? Manager { get; set; }

            [Required]
            public string Name { get; set; } = string.Empty;

            [Required]
            public DateTime? DateOfStart { get; set; }

            [Required]
            public DateTime? DueDate { get; set; }

            public bool IsTimeAndMaterial { get; set; }

            [Required]
            public PositiveMoney Price { get; set; } = new() { Currency = "EUR", Amount = 0 };

            public string? Description { get; set; }

            public string? PurchaseOrderNumber { get; set; }
        }
    }
}
