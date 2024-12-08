using Market.Models;

namespace Market.Interfaces
{
    public interface IVoucherOrderService
    {
        Result CreateTimeLimitedOfferOrder(string voucherId);
    }
}