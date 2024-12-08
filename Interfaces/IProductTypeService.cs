using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductTypeService
    {
        Page<ProductType> GetProductTypeList(SystemProductTypePage req);
    }
}