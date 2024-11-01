using System.Threading.Tasks;

namespace CampusMarketApi.Core.Interfaces;

public interface ITokenClaimService
{
    Task<string> GetTokenClaim(string username);
}