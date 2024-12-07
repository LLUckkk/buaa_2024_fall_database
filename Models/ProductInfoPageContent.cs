namespace Market.Models
{
    public class ProductInfoPageContent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public long OriginalPrice { get; set; }
        public int PostType { get; set; }
        public string Adcode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
        public long CreateTime { get; set; }
        public string Avatar { get; set; }
        public int LikeCount { get; set; }
        public long UpdateTime { get; set; }
    }
}