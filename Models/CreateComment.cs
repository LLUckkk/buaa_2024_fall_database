namespace Market.Models
{
    public class CreateComment
    {
        public required string ProductId { get; set; }
        public string? ParentId { get; set; }
        public required string Content { get; set; }
    }
}