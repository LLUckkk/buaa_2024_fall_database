namespace Market.Models
{
    public class ChatListCreate
    {
        public string? FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string ProductId { get; set; }
        public string ProductImage { get; set; }
    }
}