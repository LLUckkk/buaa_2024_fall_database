using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities {
    [Table("chat_list")]
    public class ChatList {
        [Key]
        [Column("id")]
        [StringLength(36)]
        public string Id { get; set; }

        [Column("product_id")]
        [StringLength(36)]
        public string ProductId { get; set; }

        [Column("product_image")]
        [StringLength(255)]
        public string ProductImage { get; set; }

        [Column("from_user_id")]
        [StringLength(36)]
        [Required]
        public string FromUserId { get; set; }

        [Column("from_user_avatar")]
        [StringLength(100)]
        public string FromUserAvatar { get; set; }

        [Column("from_user_nick")]
        [StringLength(100)]
        public string FromUserNick { get; set; }

        [Column("to_user_id")]
        [StringLength(36)]
        [Required]
        public string ToUserId { get; set; }

        [Column("to_user_nick")]
        [StringLength(100)]
        public string ToUserNick { get; set; }

        [Column("to_user_avatar")]
        [StringLength(100)]
        public string ToUserAvatar { get; set; }

        [Column("create_time")]
        public long CreateTime { get; set; }

        [Column("update_time")]
        public long UpdateTime { get; set; }
    }
}