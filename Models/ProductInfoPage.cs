namespace Market.Models
{
    public class ProductInfoPage : Page<ProductInfo>
    {
        public string TypeCode { get; set; }
        public string Key { get; set; }
        public string Status { get; set; }
    }
}