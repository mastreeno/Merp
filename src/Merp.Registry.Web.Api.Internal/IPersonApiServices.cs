using Merp.Registry.Web.Api.Internal.Models;

namespace Merp.Registry.Web.Api.Internal
{
    public interface IPersonApiServices
    {
        Task<IEnumerable<PersonInfoModel>> SearchPeopleByPatternAsync(string query);

        Task RegisterPersonAsync(RegisterPersonModel model);
    }
}
