using Market.Entities;

namespace Market.Models
{
    public class SystemRolePage : Page<SystemRole>
    {
        public string Key { get; set; }
        public string Status { get; set; }
    }
}