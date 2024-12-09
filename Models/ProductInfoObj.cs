namespace Market.Models
{
    public class ProductInfoObj
    {
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double OriginalPrice { get; set; }
        public int PostType { get; set; }
        public string Adcode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string? Type { get; set; }
    }
}