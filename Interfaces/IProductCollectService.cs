using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductCollectService
    {
        Result Create(string req);
        Result Delete(string id);
        Result<List<string>> GetCollectionProductIdList();
    }
}