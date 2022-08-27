using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class PricesByVatEditor
    {
        [Parameter]
        public List<InvoicePriceByVat> PricesByVat { get; set; } = new();
    }
}
