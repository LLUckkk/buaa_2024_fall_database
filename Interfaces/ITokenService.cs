namespace Market.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userid, string role);
        string? ValidateToken(string token);
        string GetCurrentLoginUserId();
        void LogoutCurrentUser(bool isSystemUser);
        void ClearOldToken();
    }

}