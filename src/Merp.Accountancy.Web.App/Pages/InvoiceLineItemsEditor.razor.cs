using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class InvoiceLineItemsEditor
    {
        [Parameter]
        public List<InvoiceLineItem> LineItems { get; set; } = new();

        private void AddNewLineItem() => LineItems.Add(new());

        private void RemoveLineItem(InvoiceLineItem item) => LineItems.Remove(item);

        private void CalculateTotalPrice(InvoiceLineItem item)
        {
            item.TotalPrice = item.UnitPrice * item.Quantity;
        }
    }
}
