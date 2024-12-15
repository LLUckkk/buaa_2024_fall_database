using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductOrderService
    {
        Result<string> CreateProductOrder(string productId, string info, string address, string name, string phone);
        void ClearOutdatedOrder(ProductOrder order);
        Result<List<ProductOrder>> GetMySellOrderList();
        Result<List<ProductOrder>> GetMyBuyOrderList();

        Result<Page<ProductOrder>> GetProductOrderList(SystemProductOrderPage req);
        Result<ProductOrderDetail> GetProductOrderDetail(string orderId);
        Result<Page<ProductOrder>> GetProductOrderToBeApprovedList(SystemProductOrderPage req);
        Result<long> GetTodayCount();
        Result<long> GetMonthCount();
        Result<long> GetTodayTurnover();
        Result<long> GetMonthTurnover();

        Result UserPerformSelfPickup(string orderId);
        Result UserPerformDelivery(ProductOrderPost req);
        Result UserConfigurePickupAddress(ProductOrderPost req);
        Result UserConfirmDelivery(string orderId);
        Result UserFeedback(ProductOrderEvaluate req);

        Result<UserStatistics> GetUserStat();

        // List<Map> getTableData();
        // List<Map<String, Object>> getVideoData();
        // Map<String, List<Map<String, Long>>> getOrderData();
    }
}