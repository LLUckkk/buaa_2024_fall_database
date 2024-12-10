namespace Market.Models
{
    public class SystemProductOrderPage : Page<ProductOrderObj>
    {
        public string? Key { get; set; }
        public string? Status { get; set; }
    }
}