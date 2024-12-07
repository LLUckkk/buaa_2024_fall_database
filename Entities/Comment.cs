using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("product_id")]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductInfo Product { get; set; }

        [Column("parent_id")]
        public string ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Comment ParentComment { get; set; }

        [Column("pub_user_id")]
        public string PubUserId { get; set; }

        [ForeignKey("PubUserId")]
        public User PubUser { get; set; }

        [Column("parent_user_id")]
        public string ParentUserId { get; set; }

        [ForeignKey("ParentUserId")]
        public User ParentUser { get; set; }

        [Column("pub_nickname")]
        public string PubNickname { get; set; }

        [Column("parent_user_nickname")]
        public string ParentUserNickname { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }
    }
}