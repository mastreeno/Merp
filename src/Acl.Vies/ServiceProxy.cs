using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acl.Vies
{
    public class ServiceProxy
    {
        public async Task<CompanyInformation> LookupCompanyInfoByViesServiceAsync(string countryCode, string vatNumber)
        {

            var company = new CompanyInformation();

            return company;
        }
    }
}
