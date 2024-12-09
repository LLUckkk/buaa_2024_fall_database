using Market.Interfaces;

namespace Market.Services
{
    public class ProductOrderService(ApplicationDbContext dbContext, IUserService userService) : IProductOrderService {
        public Result<string> CreateProductOrder(string productId, UserAddress public address, string extraInfo);
        public void ClearOutdatedOrders();
        public Result<List<ProductOrder>> GetMySellOrderList();
        public Result<List<ProductOrder>> GetMyBuyOrderList();
        public Page<ProductOrder> GetProductOrderList(SystemProductOrderPage req);
        public Result<ProductOrderDetail> GetProductOrderDetail(string orderId);
        public Page<ProductOrder> GetProductOrderToBeApprovedListpublic (SystemProductOrderPage req);
        public Result<long> GetTodayCount();
        public Result<long> GetMonthCount();
        public Result<long> GetTodayTurnover();
        public Result<long> GetMonthTurnover();
    }
}