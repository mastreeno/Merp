using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class PricesByVatEditor
    {
        [Parameter]
        public List<InvoicePriceByVat> PricesByVat { get; set; } = new();

        [Parameter]
        public EventCallback OnPriceByVatChange { get; set; }

        private async Task OnVatAmountChange(decimal value, InvoicePriceByVat item)
        {
            item.VatAmount = value;
            await OnPriceByVatChange.InvokeAsync();
        }

        private async Task OnTotalPriceChange(decimal value, InvoicePriceByVat item)
        {
            item.TotalPrice = value;
            await OnPriceByVatChange.InvokeAsync();
        }
    }
}
