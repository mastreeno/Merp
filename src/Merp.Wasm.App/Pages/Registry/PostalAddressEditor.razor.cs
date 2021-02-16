using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Wasm.App.Pages.Registry
{
    public partial class PostalAddressEditor
    {
        [Parameter]
        public Merp.Wasm.App.Model.PostalAddress Address { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }
    }
}
