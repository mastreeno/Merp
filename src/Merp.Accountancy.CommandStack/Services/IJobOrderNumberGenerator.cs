using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Services
{
    public interface IJobOrderNumberGenerator
    {
        string Generate();
    }
}
