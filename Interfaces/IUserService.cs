using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IUserService {
        Result<string> Register(UserRegister req);
        Result<string> Login(UserLogin req);
        Result<User> GetUserInfo();
        Result<User> GetUserInfo(string id);
        Result<Page<User>> getUserList(UserAdminPage req);
        Result UpdateUserInfo(UpdateUserInfo req);
        Result UpdateUserPassword(UpdateUserInfo req);
    }
}