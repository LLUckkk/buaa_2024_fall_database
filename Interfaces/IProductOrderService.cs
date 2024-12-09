using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IProductOrderService
    {
        Result<string> CreateProductOrder(string productId, UserAddress address, string extraInfo);
        void ClearOutdatedOrders();
        Result<List<ProductOrder>> GetMySellOrderList();
        Result<List<ProductOrder>> GetMyBuyOrderList();

        Page<ProductOrder> GetProductOrderList(SystemProductOrderPage req);
        Result<ProductOrderDetail> GetProductOrderDetail(string orderId);
        Page<ProductOrder> GetProductOrderToBeApprovedList(SystemProductOrderPage req);
        Result<long> GetTodayCount();
        Result<long> GetMonthCount();
        Result<long> GetTodayTurnover();
        Result<long> GetMonthTurnover();

        // List<Map> getTableData();
        // List<Map<String, Object>> getVideoData();
        // Map<String, List<Map<String, Long>>> getOrderData();
    }
}