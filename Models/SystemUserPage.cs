using Market.Entities;

namespace Market.Models
{
    public class SystemUserPage : Page<SystemUser>
    {
        public string Key { get; set; }
        public string Status { get; set; }
    }
}