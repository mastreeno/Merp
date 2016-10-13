using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Services
{
    public class JobOrderNumberGenerator : IJobOrderNumberGenerator
    {
        public string Generate()
        {
            return string.Format("{0}-{1:ddMM}/{1:yyyy}", Guid.NewGuid().GetHashCode(), DateTime.Now);
        }
    }
}
