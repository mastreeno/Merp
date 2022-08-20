using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Merp.Accountancy.Web.App.Pages
{
    public partial class PersonInfoAutocomplete
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

        private Task<IEnumerable<ViewModel>> SearchPeopleByTextAsync(string text)
        {
            return Task.Run(() =>
            {
                var customers = new[]
                {
                    new ViewModel { Id = 1, OriginalId = Guid.NewGuid(), Name = "Alberto Mori" },
                    new ViewModel { Id = 2, OriginalId = Guid.NewGuid(), Name = "Paolino Paperino" },
                    new ViewModel { Id = 3, OriginalId = Guid.NewGuid(), Name = "Gastone Paperone" },
                    new ViewModel { Id = 4, OriginalId = Guid.NewGuid(), Name = "Paperon de Paperoni" },
                    new ViewModel { Id = 5, OriginalId = Guid.NewGuid(), Name = "Renè Ferretti" },
                    new ViewModel { Id = 6, OriginalId = Guid.NewGuid(), Name = "Stanis La Rochelle" },
                    new ViewModel { Id = 7, OriginalId = Guid.NewGuid(), Name = "Corinna Negri" },
                };

                if (string.IsNullOrWhiteSpace(text))
                {
                    return customers;
                }

                return customers.Where(c => c.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase));
            });
        }

        public class ViewModel
        {
            public int Id { get; set; }

            public Guid OriginalId { get; set; }

            public string Name { get; set; } = string.Empty;
        }
    }
}
