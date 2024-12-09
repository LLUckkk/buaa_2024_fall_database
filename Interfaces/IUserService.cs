using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IUserService {
        Result<string> Register(UserRegister req);
        Result<string> Login(UserLogin req);
        Result<User> GetUser();
        Page<User> getUserList(UserAdminPage req);
        Result UpdateUserInfo(UpdateUserInfo req);
        Result UpdateUserPassword(UpdateUserInfo req);
        User? GetCurrentUser();
        User? GetUserById(string id);
    }
}