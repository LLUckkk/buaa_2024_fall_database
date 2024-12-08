using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface ICommentService
    {
        CommentListObj GetCommentList(string productId);

        Result CreateComment(string productId, string? parentId, string content);

        Page<Comment> GetCommentPage(SystemCommentPage req);
    }
}