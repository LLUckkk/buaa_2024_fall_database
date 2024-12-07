namespace Market.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string PubUserId { get; set; }
        public string PubNickname { get; set; }
        public string PubProvince { get; set; }
        public string PubAvatar { get; set; }
        public string ParentUserId { get; set; }
        public string ParentUserNickname { get; set; }
        public string Content { get; set; }
        public long CreateTime { get; set; }
        public List<Comment> CommentChildren { get; set; }
    }
}