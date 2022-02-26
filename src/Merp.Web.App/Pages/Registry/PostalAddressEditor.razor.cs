using Microsoft.AspNetCore.Components;

namespace Merp.Web.App.Pages.Registry
{
    public partial class PostalAddressEditor
    {
        [Parameter]
        public Merp.Web.App.Model.PostalAddress Address { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }
    }
}
