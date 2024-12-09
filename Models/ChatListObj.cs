using Market.Entities;

namespace Market.Models
{
    public class ChatListObj
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
    
        public static ChatListObj FromChatList(ChatList list) {
            return new ChatListObj {
                Id = list.Id,
                ProductId = list.ProductId,
                ProductImage = list.ProductImage,
                FromUserId = list.FromUserId,
                ToUserId = list.ToUserId,
                FromUserNick = list.FromUserNickname,
                ToUserNick = list.ToUserNickname,
                FromUserAvatar = list.FromUserAvatar,
                ToUserAvatar = list.ToUserAvatar,
                CreateTime = list.CreateTime,
                UpdateTime = list.UpdateTime,
            };
        }
    }
}