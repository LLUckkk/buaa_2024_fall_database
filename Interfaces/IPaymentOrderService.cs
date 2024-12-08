using Market.Models;

namespace Market.Interfaces
{
    public interface IPaymentOrderService
    {
        Result<string> CreatePaymentOrder(string productId);
        void PaymentStatusUpdateCallback(PaymentOrder order); 
        Page<PaymentOrder> GetPaymentOrderPage(SystemPaymentOrderPage req);
        Result<PaymentOrderAdminDetail> GetPaymentOrderDetail(string orderId);
        Result<string> CreatePay(string orderId);
        Result FinishPay(string payId);
        void PayStatusUpdateCallback(PaymentPay pay);
    
    }
}