using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IPaymentService
    {
        Result<string> CreatePaymentOrder(string productOrderId);
        Result<PaymentOrder> GetPaymentOrderById(string orderId);
        void PaymentOrderStatusUpdateCallback(PaymentOrder order);
        Result<Page<PaymentOrder>> GetPaymentOrderPage(SystemPaymentOrderPage req);
        Result<PaymentOrderAdminDetail> GetPaymentOrderDetail(string orderId);
        Result<string> CreatePay(string orderId);
        Result FinishPay(string payId);
        void PayStatusUpdateCallback(PaymentPay pay);

    }
}