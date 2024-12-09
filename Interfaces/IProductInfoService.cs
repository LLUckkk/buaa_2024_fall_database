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
        Result<ProductInfoAdminDetail> GetProductInfoAdminDetail(string id);
        Result<List<ProductInfoDetail>> GetMyProductCollectionInfoList();
        Result ApproveProduct(string id);
        Result RejectProduct(string id);
        Result HideProduct(string id);
        Result<long> GetTodayCount();
        Result<long> GetMonthCount();
    }
}