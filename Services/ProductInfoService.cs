using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductInfoService(ApplicationDbContext dbContext, IUserService userService) : IProductInfoService
    {
        public Result CreateProductInfo(ProductInfo req) {
            
        }
        public List<ProductInfoPageContent> GetProductInfoList(ProductInfoPage req);
        public Result<ProductInfoDetail> GetProductInfo(string id);
        public List<ProductInfo> GetMyProductInfoList();
        public Result CreateLike(string id);
        public Page<ProductInfo> GetProductInfoPage(SystemProductInfoPage req);
        public Result<ProductInfoAdminDetail> GetProductInfoAdminDetail(string id);
        public Result<List<ProductInfo>> GetMyProductCollectionInfoList();
        public Result ApproveProduct(string id);
        public Result RejectProduct(string id);
        public Result HideProduct(string id);
        public Result<long> GetTodayCount();
        public Result<long> GetMonthCount();
    }
}