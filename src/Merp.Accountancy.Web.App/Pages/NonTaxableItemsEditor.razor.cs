using Merp.Accountancy.Web.App.Model;
using Microsoft.AspNetCore.Components;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class NonTaxableItemsEditor
    {
        [Parameter]
        public List<NonTaxableItem> NonTaxableItems { get; set; } = new();

        private void AddNewNonTaxableItem() => NonTaxableItems.Add(new());

        private void RemoveNonTaxableItem(NonTaxableItem item) => NonTaxableItems.Remove(item);
    }
}
