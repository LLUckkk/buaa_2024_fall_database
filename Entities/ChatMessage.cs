using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Market.Entities
{
    [Table("chat_message")]
    public class ChatMessage
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("chat_list_id")]
        public string ChatListId { get; set; }

        [Column("from_user_id")]
        public string FromUserId { get; set; }

        [Column("to_user_id")]
        public string ToUserId { get; set; }

        [Column("from_user_nick")]
        public string FromUserNick { get; set; }

        [Column("to_user_nick")]
        public string ToUserNick { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("send_time")]
        public long SendTime { get; set; }

        [Column("is_read")]
        public int IsRead { get; set; }
    }
}