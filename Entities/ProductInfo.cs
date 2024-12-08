using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("product_info")]
    public class ProductInfo
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("intro")]
        public string Intro { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("price")]
        public long Price { get; set; }

        [Column("original_price")]
        public long OriginalPrice { get; set; }

        [Column("type_code")]
        public string TypeCode { get; set; }

        [Column("type_name")]
        public string TypeName { get; set; }

        [Column("post_type")]
        public int PostType { get; set; }

        [Column("like_count")]
        public int LikeCount { get; set; }

        [Column("adcode")]
        public string Adcode { get; set; }

        [Column("province")]
        public string Province { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("district")]
        public string District { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}