using Merp.Registry.Web.Api.Internal.Models;

namespace Merp.Registry.Web.Api.Internal
{
    public interface IPartyApiServices
    {
        Task<IEnumerable<PartyInfoModel>> GetPartyInfoByPatternAsync(string query);

        Task<PartyInfoModel?> GetPartyInfoByIdAsync(Guid partyId);

        Task RegisterPartyAsync(RegisterPartyModel model);

        Task<PartyBillingInfo?> GetPartyBillingInfoByPartyIdAsync(Guid partyId);
    }
}
