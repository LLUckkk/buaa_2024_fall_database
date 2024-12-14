using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IUserAddressService
    {
        Result AddUserAddress(UserAddressObj req);
        Result UpdateUserAddress(UserAddressObj req);
        Result DeleteUserAddress(string id);
        Result<List<UserAddressObj>> GetUserAddressList();
    }
}