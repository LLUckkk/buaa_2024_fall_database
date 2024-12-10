using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductTypeService
    {
        Page<ProductType> GetProductTypeList(SystemProductTypePage req);
        List<ProductType> GetList();
        Result CreateProductType(ProductTypeObj req);
        Result UpdateProductType(ProductTypeObj req);
    }
}