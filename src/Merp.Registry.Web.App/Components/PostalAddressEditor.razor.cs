using Microsoft.AspNetCore.Components;

namespace Merp.Registry.Web.App.Components
{
    public partial class PostalAddressEditor
    {
        [Parameter]
        public Merp.Registry.Web.App.Model.PostalAddress Address { get; set; } = new();

        [Parameter]
        public bool IsDisabled { get; set; }
    }
}
