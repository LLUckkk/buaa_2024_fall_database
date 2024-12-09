using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductInfoService(ApplicationDbContext dbContext, IUserService userService) : IProductInfoService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public Result CreateProductInfo(ProductInfoObj req) {
            
        }
        public List<ProductInfoPageContent> GetProductInfoList(ProductInfoPage req);
        public Result<ProductInfoDetail> GetProductInfo(string id);
        public List<ProductInfoObj> GetMyProductInfoList();
        public Result CreateLike(string id);
        public Page<ProductInfoObj> GetProductInfoPage(SystemProductInfoPage req);
        public Result<ProductInfoAdminDetail> GetProductInfoAdminDetail(string id);
        public Result<List<ProductInfoObj>> GetMyProductCollectionInfoList();
        public Result ApproveProduct(string id);
        public Result RejectProduct(string id);
        public Result HideProduct(string id);
        public Result<long> GetTodayCount();
        public Result<long> GetMonthCount();
    }
}