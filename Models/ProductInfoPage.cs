namespace Market.Models
{
    public class ProductInfoPage : Page<ProductInfoObj>
    {
        public string TypeCode { get; set; }
        public string Key { get; set; }
        public string Status { get; set; }
    }
}