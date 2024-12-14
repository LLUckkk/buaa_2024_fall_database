using Market.Entities;

namespace Market.Models
{
    public class ProductInfoDetail
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Intro { get; set; }
        public required string Image { get; set; }
        public long Price { get; set; }
        public long OriginalPrice { get; set; }
        public int PostType { get; set; }
        public string? Adcode { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public int Status { get; set; }
        public required string UserId { get; set; }
        public int LikeCount { get; set; }
        public long CreateTime { get; set; }
        public User? UserInfo { get; set; }
        public long UpdateTime { get; set; }
        public ProductVoucher? ProductVoucher { get; set; }
        public required string Type { get; set; }
    }
}