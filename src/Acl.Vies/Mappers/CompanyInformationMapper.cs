using Acl.Vies.Vies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.Vies.Mappers
{
    internal abstract class CompanyInformationMapper
    {
        internal virtual bool CanMap(checkVatResponse body)
        {
            return body != null && body.valid;
        }

        internal virtual CompanyInformation Map(checkVatResponse body)
        {
            if(body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var companyInformation = new CompanyInformation
            {
                CompanyName = body.name,
                VatNumber = body.vatNumber,
                Country = body.countryCode
            };

            if (CanMapAddress(body.address))
            {
                MapAddress(companyInformation, body.address);
            }

            return companyInformation;
        }

        protected abstract void MapAddress(CompanyInformation companyInformation, string address);

        protected virtual bool CanMapAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address);
        }
    }
}
