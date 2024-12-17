using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductInfoService
    {
        Result CreateProductInfo(ProductInfoObj req);
        Result<List<ProductInfoDetail>> GetProductInfoList(ProductInfoPage req);
        Result<ProductInfoDetail> GetProductInfo(string id);
        Result<List<ProductInfo>> GetMyProductInfoList();
        Result CreateLike(string id);
        Result<Page<ProductInfoDetail>> GetProductInfoPage(SystemProductInfoPage req);
        Result<List<ProductInfoDetail>> GetMyProductCollectionInfoList();
        Result RemoveById(string id);
        Result HideProduct(string id);
        Result ShowProduct(string id);
        Result ModifyProductStatus(string id, int status);
        Result<long> GetTodayCount();
        Result<long> GetMonthCount();
    }
}