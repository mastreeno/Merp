using Acl.RegistryResolutionServices.Vies;
using System;
using System.Linq;

namespace Acl.RegistryResolutionServices.Mappers
{
    internal abstract class PersonInformationMapper
    {
        internal virtual bool CanMap(checkVatResponse body)
        {
            return body != null && body.valid;
        }

        internal virtual PersonInformation Map(checkVatResponse body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var person = new PersonInformation
            {
                VatNumber = body.vatNumber,
                Country = body.countryCode
            };

            if (CanMapName(body.name))
            {
                MapName(person, body.name);
            }

            if (CanMapAddress(body.address))
            {
                MapAddress(person, body.address);
            }

            return person;
        }

        protected abstract void MapAddress(PersonInformation person, string address);

        protected virtual bool CanMapAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address);
        }

        protected virtual bool CanMapName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        protected virtual void MapName(PersonInformation person, string name)
        {
            var nameParts = name.Split(new[] { " " }, StringSplitOptions.None);

            string lastName = nameParts.Length > 0 ? nameParts[0].Trim() : null;
            string firstName = null;
            if (nameParts.Length > 1)
            {
                firstName = string.Join(" ", nameParts.AsEnumerable().Skip(1).ToArray()).Trim();
            }

            person.FirstName = firstName;
            person.LastName = lastName;
        }
    }
}
