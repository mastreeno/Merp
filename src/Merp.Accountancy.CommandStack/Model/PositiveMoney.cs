using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Accountancy.CommandStack.Model
{
    public class PositiveMoney : Money
    {
        public PositiveMoney(decimal amount, string currency) : base(amount, currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
        }
    }
}
