﻿using Microsoft.AspNetCore.Components;
using Rebus.Bus;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class IssueOutgoingInvoice
    {
        [Inject] public IBus Bus { get; set; } = default!;

        private IssueInvoiceForm.ViewModel model = new();

        private async Task IssueOutgoingInvoiceAsync(IssueInvoiceForm.ViewModel invoice)
        {
            //TODO issue command
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
