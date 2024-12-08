using System.ComponentModel.DataAnnotations.Schema;
using Market.Entities;

namespace Market.Models
{
    public class CommentObj
    {
        public string Id { get; set; }
        public string? ParentId { get; set; }
        public string PubUserId { get; set; }
        public string PubNickname { get; set; }
        public string PubAvatar { get; set; }
        public string? ParentUserId { get; set; }
        public string? ParentUserNickname { get; set; }
        public string Content { get; set; }
        public long CreateTime { get; set; }
        public List<CommentObj> CommentChildren { get; set; }
    }
}