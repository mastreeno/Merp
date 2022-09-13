using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class NonTaxableItemsEditor
    {
        [Parameter]
        public List<NonTaxableItem> NonTaxableItems { get; set; } = new();

        [Parameter]
        public EventCallback OnNonTaxableItemsChange { get; set; }

        private void AddNewNonTaxableItem() => NonTaxableItems.Add(new());

        private async Task RemoveNonTaxableItemAsync(NonTaxableItem item)
        {
            NonTaxableItems.Remove(item);
            await OnNonTaxableItemsChange.InvokeAsync();
        }

        private async Task OnAmountChange(decimal value, NonTaxableItem item)
        {
            item.Amount = value;
            await OnNonTaxableItemsChange.InvokeAsync();
        }
    }
}
