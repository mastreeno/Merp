using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class WithholdingTaxDisplay
    {
        [Parameter]
        public WithholdingTax WithholdingTax { get; set; } = new();

        [Parameter]
        public string Currency { get; set; } = string.Empty;
    }
}
