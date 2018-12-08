using System;

namespace Merp.Accountancy.CommandStack.Services
{
    public class OutgoingCreditNoteNumberGenerator : IOutgoingCreditNoteNumberGenerator
    {
        public string Generate()
        {
            return string.Format("NC/{0}/{1:yyyy}", Math.Abs(Guid.NewGuid().GetHashCode()), DateTime.Now);
        }
    }
}
