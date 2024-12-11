using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface ISystemUserService
    {
        Result<Page<SystemRole>> GetRolePage(SystemRolePage req);
        Result CreateRole(SystemRoleCreate req);
        Result DeleteRole(string id);
        Result UpdateRole(SystemRoleUpdate req);
        Result<List<SystemRole>> GetRoleList();
        Result<string> Login(SystemUserLogin req);
        Result<SystemUserInfo> GetUserInfo();
        Result<Page<SystemUserInfo>> GetUserList(SystemUserPage req);
        Result CreateSystemUser(SystemUserCreate req);
        Result UpdateSystemUser(SystemUserUpdate req);

        Result DeleteSystemUser(string id);
    }
}