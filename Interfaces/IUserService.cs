using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IUserService {
        Result<string> Register(UserRegister req);
        Result<string> Login(UserLogin req);
        Result<User> GetUser();
        Result<string> GetValidateToken(string email);
        Result ResetPassword(ResetPasswordObj req);
        Page<User> GetUserList(UserAdminPage req);
        Result UpdateUserInfo(UpdateUserInfo req);
        Result UpdateUserPassword(UpdateUserInfo req);
        User? GetCurrentUser();
        User? GetUserById(string id);
        Result EnableUser(string id);
        Result DisableUser(string id);
        Result ApproveUserProfileUpdate(string id);
        Result RejectUserProfileUpdate(string id);
    }
}