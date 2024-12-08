using Market.Entities;

namespace Market.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string user);
        string? ValidateToken(string token);
        string? GetCurrentLoginUserId();
        void LogoutCurrentUser();
        void ClearOldToken();
    }

}