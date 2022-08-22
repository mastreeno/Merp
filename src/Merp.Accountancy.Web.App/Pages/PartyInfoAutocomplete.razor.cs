using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class PartyInfoAutocomplete
    {
        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public int DebounceInterval { get; set; } = 300;

        [Parameter]
        public Func<ViewModel?, string> DisplayValueTemplate { get; set; } = x => x?.Name ?? string.Empty;

        [Parameter]
        public ViewModel? Value { get; set; }

        [Parameter]
        public EventCallback<ViewModel> ValueChanged { get; set; }

        public Expression<Func<ViewModel?>> ValueExpression { get; set; }

        private Task<IEnumerable<ViewModel>> SearchPartyByTextAsync(string text)
        {
            return Task.Run(() => Enumerable.Empty<ViewModel>());
        }

        public class ViewModel
        {
            public int Id { get; set; }

            public Guid OriginalId { get; set; }

            public string Name { get; set; } = string.Empty;
        }
    }
}
