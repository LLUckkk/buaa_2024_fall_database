namespace Market.Models
{
    public class ChatMessageWsObj
    {
        public required string FromUserId { get; set; }
        public required string ToUserId { get; set; }
        public required string Content { get; set; }
        public required string ChatListId { get; set; }
        public long SendTime { get; set; }
    }
}