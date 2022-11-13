using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class InvoiceLineItemsEditor
    {
        [Parameter]
        public List<InvoiceLineItem> LineItems { get; set; } = new();

        [Parameter]
        public IEnumerable<Vat> VatList { get; set; } = Array.Empty<Vat>();

        [Parameter]
        public EventCallback OnLineItemsChange { get; set; }

        private void AddNewLineItem() => LineItems.Add(new());

        private async Task RemoveLineItemAsync(InvoiceLineItem item)
        {
            LineItems.Remove(item);
            await OnLineItemsChange.InvokeAsync();
        }

        private async Task CalculateTotalPriceAsync(InvoiceLineItem item)
        {
            item.TotalPrice = item.UnitPrice * item.Quantity;
            await OnLineItemsChange.InvokeAsync();
        }

        private async Task OnItemQuantityChange(int value, InvoiceLineItem item)
        {
            item.Quantity = value;
            await CalculateTotalPriceAsync(item);
        }

        private async Task OnUnitPriceChange(decimal value, InvoiceLineItem item)
        {
            item.UnitPrice = value;
            await CalculateTotalPriceAsync(item);
        }

        private async Task OnTotalPriceChange(decimal value, InvoiceLineItem item)
        {
            item.TotalPrice = value;
            await OnLineItemsChange.InvokeAsync();
        }

        private async Task OnVatChange(Vat value, InvoiceLineItem item)
        {
            item.Vat = value;
            await OnLineItemsChange.InvokeAsync();
        }
    }
}
