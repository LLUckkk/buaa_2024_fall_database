using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IUserAddressService
    {
        Result AddUserAddress(UserAddress req);
        Result DeleteUserAddress(string id);
        IEnumerable<UserAddress> GetUserAddressList();
    }
}