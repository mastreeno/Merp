using System;

namespace Merp.Web
{
    public interface IAppContext
    {
        Guid UserId { get; }
    }
}
