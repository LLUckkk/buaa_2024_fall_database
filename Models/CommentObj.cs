using System.ComponentModel.DataAnnotations.Schema;
using Market.Entities;

namespace Market.Models
{
    public class CommentObj
    {
        public required string Id { get; set; }
        public string? ParentId { get; set; }
        public required string PubUserId { get; set; }
        public required string PubNickname { get; set; }
        public required string PubAvatar { get; set; }
        public string? ParentUserId { get; set; }
        public string? ParentUserNickname { get; set; }
        public required string Content { get; set; }
        public long CreateTime { get; set; }
        public List<CommentObj>? CommentChildren { get; set; }
    }
}