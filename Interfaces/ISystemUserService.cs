using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface ISystemUserService
    {
        Page<SystemRole> GetRoleList(SystemRolePage req);
        Result<string> Login(SystemUserLogin req);
        Result<SystemUserInfo> GetUserInfo();
        Page<SystemUserInfo> GetUserList(SystemUserPage req);
        Result Create(SystemUserCreate req);
        Result Update(SystemUserUpdate req);
    }
}