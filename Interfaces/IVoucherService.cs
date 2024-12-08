using Market.Models;

namespace Market.Interfaces
{
    public interface IVoucherService
    {
        Result CreateProductVoucher(ProductVoucherCreate req);
        Result CreateTimeLimitedOfferOrder(string voucherId);
    }
}