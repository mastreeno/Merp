using Microsoft.AspNetCore.Components;

namespace Merp.Registry.Web.App.Pages
{
    public partial class PostalAddressEditor
    {
        [Parameter]
        public Merp.Registry.Web.App.Model.PostalAddress Address { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }
    }
}
