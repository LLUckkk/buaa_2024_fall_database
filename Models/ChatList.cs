using Market.Entities;

namespace Market.Models
{
    public class ChatList
    {
        public string Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string FromUserNick { get; set; }
        public string FromUserAvatar { get; set; }
        public string ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ToUserNick { get; set; }
        public string ToUserAvatar { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
        public ChatMessage ChatMessage { get; set; }
        public int NoReadCount { get; set; }
    }
}