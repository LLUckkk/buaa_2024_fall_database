using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class PaymentOrderService(IUserService userService, IPaymentService paymentService, IProductOrderService productOrderService) : IPaymentOrderService
    {
        private readonly IUserService _userService = userService;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly IProductOrderService _productOrderService = productOrderService;
        Result<string> CreatePaymentOrder(string productId);
        void PaymentStatusUpdateCallback(PaymentOrder order);
        Page<PaymentOrder> GetPaymentOrderPage(SystemPaymentOrderPage req);
        Result<PaymentOrderAdminDetail> GetPaymentOrderDetail(string orderId);
    }
}