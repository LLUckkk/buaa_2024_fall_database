using Market.Entities;

namespace Market.Models
{
    public class ProductOrderDetail {
        public ProductOrder ProductOrder { get; set; }
        public ProductInfo ProductInfo { get; set; }
        public PaymentOrder PaymentOrder { get; set; }
        public PaymentPay PaymentPay { get; set; }
    }
}