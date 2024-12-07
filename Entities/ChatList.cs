using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities {
    [Table("chat_list")]
    public class ChatList {
        [Key]
        [Column("id")]
        [MaxLength(19)]
        public string Id { get; set; }

        [ForeignKey("ProductInfo")]
        [Column("product_id")]
        [MaxLength(19)]
        public string ProductId { get; set; }

        [Column("product_image")]
        [MaxLength(255)]
        public string ProductImage { get; set; }

        [Column("from_user_id")]
        [MaxLength(19)]
        [Required]
        public string FromUserId { get; set; }

        [Column("from_user_avatar")]
        [MaxLength(100)]
        public string FromUserAvatar { get; set; }

        [Column("from_user_nick")]
        [MaxLength(100)]
        public string FromUserNick { get; set; }

        [Column("to_user_id")]
        [MaxLength(19)]
        [Required]
        public string ToUserId { get; set; }

        [Column("to_user_nick")]
        [MaxLength(100)]
        public string ToUserNick { get; set; }

        [Column("to_user_avatar")]
        [MaxLength(100)]
        public string ToUserAvatar { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}