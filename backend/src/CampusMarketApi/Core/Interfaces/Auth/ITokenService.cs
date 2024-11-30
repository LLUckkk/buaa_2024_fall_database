using CampusMarketApi.Core.Models.Entities;

namespace CampusMarketApi.Core.Interfaces.Auth;

public interface ITokenService
{
    string GenerateToken(User user);
    string? ValidateToken(string token);
}