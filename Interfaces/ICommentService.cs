using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface ICommentService
    {
        Result<CommentListObj> GetCommentList(string productId);

        Result CreateComment(CreateComment req);

        Page<Comment> GetCommentPage(SystemCommentPage req);

        Result DeleteComment(string id);
    }
}