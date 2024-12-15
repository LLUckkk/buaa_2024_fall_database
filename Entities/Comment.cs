using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public required string Id { get; set; }

        [Column("product_id")]
        public required string ProductId { get; set; }

        [Column("parent_id")]
        public string? ParentId { get; set; }

        [Column("pub_user_id")]
        public required string PubUserId { get; set; }

        [Column("parent_user_id")]
        public string? ParentUserId { get; set; }

        [Column("pub_nickname")]
        public required string PubNickname { get; set; }

        [Column("parent_user_nickname")]
        public string? ParentUserNickname { get; set; }

        [Column("content")]
        public required string Content { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }
    }
}