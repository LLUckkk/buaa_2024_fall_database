using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IVoucherService
    {
        Result CreateProductVoucher(ProductVoucherCreate req);
        Result CreateTimeLimitedOfferOrder(string voucherId);
        ProductVoucher? GetProductVoucher(string id);
    }
}