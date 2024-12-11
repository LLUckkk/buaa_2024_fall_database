using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IVoucherService
    {
        Result CreateProductVoucher(ProductVoucherCreate req);
        Result CreateTimeLimitedOfferOrder(string voucherId);
        Result<ProductVoucher?> GetProductVoucher(string id);
        Result DeleteProductVoucher(string id);
    }
}