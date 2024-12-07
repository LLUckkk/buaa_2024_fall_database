using Market.Entities;

namespace Market.Models
{
    public class ProductInfoVoucher
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public long OriginalPrice { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public int? PostType { get; set; }
        public int? LikeCount { get; set; }
        public string Adcode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int? Status { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
        public ProductVoucher ProductVoucher { get; set; }
    }
}