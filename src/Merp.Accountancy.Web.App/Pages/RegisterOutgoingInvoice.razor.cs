using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterOutgoingInvoice
    {
        [Inject] public IBus Bus { get; set; } = default!;

        private RegisterOutgoingInvoiceForm.ViewModel model = new();

        private async Task RegisterOutgoingInvoiceAsync(RegisterOutgoingInvoiceForm.ViewModel invoice)
        {
            //TODO
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeModel();
        }

        private void Cancel()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            model = new();
            model.LineItems.Add(new());
        }
    }
}
