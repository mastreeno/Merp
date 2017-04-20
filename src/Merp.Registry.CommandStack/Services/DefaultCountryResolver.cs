using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Services
{
    public class DefaultCountryResolver : IDefaultCountryResolver
    {
        public string GetDefaultCountry()
        {
            return "Italy";
        }
    }
}
