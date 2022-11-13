using Merp.Accountancy.Web.App.Components;
using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class RegisterIncomingCreditNote
    {
        [Inject] public IBus Bus { get; set; } = default!;

        private RegisterIncomingInvoiceForm.ViewModel model = new();

        private async Task RegisterIncomingCreditNoteAsync(RegisterIncomingInvoiceForm.ViewModel creditNote)
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
