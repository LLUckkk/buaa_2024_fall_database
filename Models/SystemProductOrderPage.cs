namespace Market.Models
{
    public class SystemProductOrderPage : Page<ProductOrder>
    {
        public string Key { get; set; }
        public string Status { get; set; }
    }
}