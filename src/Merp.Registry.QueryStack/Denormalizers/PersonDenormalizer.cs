using Merp.Registry.CommandStack.Events;
using Merp.Registry.QueryStack.Model;
using Rebus.Handlers;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Merp.Registry.QueryStack.Denormalizers
{
    public class PersonDenormalizer : 
        IHandleMessages<PersonRegisteredEvent>
    {
        public async Task Handle(PersonRegisteredEvent message)
        {
            var p = new Person()
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                OriginalId = message.PersonId,
                DisplayName = $"{message.FirstName} {message.LastName}",
                NationalIdentificationNumber = message.NationalIdentificationNumber,
                VatIndex = message.VatNumber,
                ShippingAddress = new PostalAddress()
            };
            using (var context = new RegistryDbContext())
            {
                context.Parties.Add(p);
                await context.SaveChangesAsync();
            }
        }
    }
}
