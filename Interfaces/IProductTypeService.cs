using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductTypeService
    {
        Result<Page<ProductType>> GetProductTypeList(SystemProductTypePage req);
        Result<List<ProductType>> GetList();
        Result CreateProductType(ProductTypeObj req);
        Result UpdateProductType(ProductTypeObj req);
    }
}