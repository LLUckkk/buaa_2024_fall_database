using Market.Entities;

namespace Market.Models
{
    public class UserAdminPage : Page<User>
    {
        public string Key { get; set; }
        public int CheckStatus { get; set; }
    }
}