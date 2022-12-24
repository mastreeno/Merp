using Merp.Registry.Web.Api.Internal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using System.Linq.Expressions;

namespace Merp.Accountancy.Web.App.Components
{
    public partial class PersonInfoAutocomplete
    {
        [Inject]
        public IPersonApiServices PersonApi { get; set; } = default!;

        [Inject]
        public IDialogService Dialog { get; set; } = default!;

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

        private async Task<IEnumerable<ViewModel>> SearchPeopleByTextAsync(string text)
        {
            var people = await PersonApi.SearchPeopleByPatternAsync(text);
            return people.Select(p => new ViewModel { Id = p.Id, OriginalId = p.OriginalId, Name = p.Name });
        }

        private async Task OpenRegisterNewPersonDialogAsync()
        {
            var registerPersonResult = await Dialog.Show<RegisterPersonDialog>(
                localizer[nameof(Resources.Components.PersonInfoAutocomplete.RegisterNewPersonDialogTitle)],
                new DialogOptions
                {
                    Position = DialogPosition.Center,
                    FullWidth = true
                }).Result;

            if (!registerPersonResult.Cancelled)
            {
                var personRegistered = registerPersonResult.Data.As<RegisterPersonDialog.PersonRegistered>();
                var personFullName = $"{personRegistered.FirstName} {personRegistered.LastName}".Trim();

                var peopleFound = await SearchPeopleByTextAsync(personFullName);
                if (peopleFound.Any())
                {
                    await ValueChanged.InvokeAsync(peopleFound.FirstOrDefault());
                }
            }
        }

        public class ViewModel
        {
            public int Id { get; set; }

            public Guid OriginalId { get; set; }

            public string Name { get; set; } = string.Empty;
        }
    }
}
