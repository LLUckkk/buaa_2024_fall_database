using Market.Models;

namespace Market.Interfaces
{
    public interface IProductInfoService
    {
        Result CreateProductInfo(ProductInfo req);
        List<ProductInfoPageContent> GetProductInfoList(ProductInfoPage req);
        Result<ProductInfoDetail> GetProductInfo(string id);
        List<ProductInfo> GetMyProductInfoList();
        Result CreateLike(string id);
        Page<ProductInfo> GetProductInfoPage(SystemProductInfoPage req);
        Result<ProductInfoAdminDetail> GetProductInfoAdminDetail(string id);
        Result<List<ProductInfo>> GetMyProductCollectionInfoList();
        Result ApproveProduct(string id);
        Result RejectProduct(string id);
        Result HideProduct(string id);
        Result<long> GetTodayCount();
        Result<long> GetMonthCount();


    }
}