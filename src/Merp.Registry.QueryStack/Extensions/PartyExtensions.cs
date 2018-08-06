using Merp.Registry.QueryStack.Model;
using System.Linq;

namespace Merp.Registry.QueryStack
{
    public static class PartyExtensions
    {
        public static IQueryable<Party> NotUnlisted(this IQueryable<Party> parties)
        {
            return from p in parties
                   where !p.Unlisted
                   select p;
        }
    }
}
