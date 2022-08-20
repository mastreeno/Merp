using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;
using System.ComponentModel.DataAnnotations;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class CreateJobOrder
    {
        [Inject] public IBus Bus { get; set; } = default!;

        private ViewModel model = new();

        private async Task SubmitAsync()
        {
            //TODO
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
