using Market.Models;

namespace Market.Interfaces
{
    public interface IProductInfoService
    {
        Result CreateProductInfo(ProductInfoObj req);
        List<ProductInfoPageContent> GetProductInfoList(ProductInfoPage req);
        Result<ProductInfoDetail> GetProductInfo(string id);
        List<ProductInfoObj> GetMyProductInfoList();
        Result CreateLike(string id);
        Page<ProductInfoObj> GetProductInfoPage(SystemProductInfoPage req);
        Result<ProductInfoAdminDetail> GetProductInfoAdminDetail(string id);
        Result<List<ProductInfoObj>> GetMyProductCollectionInfoList();
        Result ApproveProduct(string id);
        Result RejectProduct(string id);
        Result HideProduct(string id);
        Result<long> GetTodayCount();
        Result<long> GetMonthCount();
    }
}