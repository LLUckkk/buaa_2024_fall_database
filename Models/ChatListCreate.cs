namespace Market.Models
{
    public class ChatListCreate
    {
        public required string ToUserId { get; set; }
        public required string ProductId { get; set; }
        public required string ProductImage { get; set; }
    }
}