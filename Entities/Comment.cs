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

        [Column("parent_id")]
        public string? ParentId { get; set; }

        [Column("pub_user_id")]
        public string PubUserId { get; set; }

        [Column("parent_user_id")]
        public string? ParentUserId { get; set; }

        [Column("pub_nickname")]
        public string PubNickname { get; set; }

        [Column("parent_user_nickname")]
        public string? ParentUserNickname { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }
    }
}