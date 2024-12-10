namespace Market.Models
{
    public class SystemProductInfoPage : Page<ProductInfoDetail>
    {
        public string? Key { get; set; }
        public string? Status { get; set; }
    }
}