using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class ConfirmDialog
    {
        [Parameter]
        public string ConfirmText { get; set; } = string.Empty;

        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; } = default!;

        private void Cancel() => Dialog.Close(DialogResult.Cancel());

        private void Confirm() => Dialog.Close(DialogResult.Ok(true));
    }
}
