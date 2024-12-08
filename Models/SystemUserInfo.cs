using Market.Entities;

namespace Market.Models
{
    public class SystemUserInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }

        public static SystemUserInfo FromSystemUser(SystemUser user)
        {
            return new SystemUserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                RoleId = user.RoleId,
                RoleCode = user.RoleCode,
                RoleName = user.RoleName,
                CreateTime = user.CreateTime,
                UpdateTime = user.UpdateTime
            };
        }
    }
}