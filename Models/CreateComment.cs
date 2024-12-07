namespace Market.Models
{
    public class CreateComment
    {
        public string ProductId { get; set; }
        public string ParentId { get; set; }
        public string Content { get; set; }
    }
}