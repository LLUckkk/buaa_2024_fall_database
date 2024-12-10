namespace Market.Models
{
    public class ProductInfoPage : Page<ProductInfoDetail>
    {
        public string? TypeCode { get; set; }
        public string? Key { get; set; }
    }
}