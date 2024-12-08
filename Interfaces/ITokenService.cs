using Market.Entities;

namespace Market.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string? ValidateToken(string token);
        string? GetCurrentLoginUserId();
        void LogoutCurrentUser();
        void ClearOldToken();
    }

}