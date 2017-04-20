using Merp.Registry.CommandStack.Events;
using Merp.Registry.QueryStack.Model;
using Rebus.Handlers;
using System.Linq;
using System.Threading.Tasks;

namespace Merp.Registry.QueryStack.Denormalizers
{
    public class CompanyDenormalizer : 
        IHandleMessages<CompanyRegisteredEvent>,
        IHandleMessages<CompanyNameChangedEvent>
    {
        public async Task Handle(CompanyRegisteredEvent message)
        {
            var p = new Company()
            {
                CompanyName = message.CompanyName,
                VatIndex = message.VatIndex,
                OriginalId = message.CompanyId,
                DisplayName = message.CompanyName,
                NationalIdentificationNumber = message.NationalIdentificationNumber ?? "",
                ShippingAddress = new PostalAddress(),
                LegalAddress = new PostalAddress(),
                BillingAddress = new PostalAddress()
            };
            using (var context = new RegistryDbContext())
            {
                context.Parties.Add(p);
                await context.SaveChangesAsync();
            }
        }

        public async Task Handle(CompanyNameChangedEvent message)
        {
            using (var context = new RegistryDbContext())
            {
                var company = (from c in context.Parties.OfType<Company>()
                               where c.OriginalId == message.CompanyId
                               select c).Single();
                company.DisplayName = message.CompanyName;
                company.CompanyName = message.CompanyName;

                await context.SaveChangesAsync();
            }
        }
    }
}
