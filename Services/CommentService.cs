using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class CommentService(IUserService userService, ApplicationDbContext dbContext) : ICommentService
    {
        private readonly IUserService _userService = userService;
        private readonly ApplicationDbContext _dbContext = dbContext;
        public Result<CommentListObj> GetCommentList(string productId)
        {
            var comments = _dbContext.Comments.Where(c => c.ProductId == productId).OrderBy(c => c.CreateTime).ToList().Select(c => new CommentObj
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Content = c.Content,
                CreateTime = c.CreateTime,
                PubUserId = c.PubUserId,
                PubNickname = c.PubNickname,
                PubAvatar = c.User.Avatar,
            }).ToList();
            var commentCount = comments.Count;
            ResolveCommentTree(comments);
            return Result<CommentListObj>.Ok(new CommentListObj
            {
                CommentList = comments,
                CommentCount = commentCount
            });
        }
        public Result CreateComment(CreateComment req)
        {
            var user = _userService.GetCurrentUser();
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = req.ProductId,
                Content = req.Content,
                PubUserId = user.Id,
                PubNickname = user.Nickname,
                CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            if (req.ParentId != null)
            {
                var parent = _dbContext.Comments.FirstOrDefault(c => c.Id == req.ParentId);
                if (parent == null)
                {
                    return Result.Fail(ResultCode.NotFoundError);
                }
                comment.ParentId = parent.ParentId ?? parent.Id;
                comment.ParentUserId = parent.PubUserId;
                comment.ParentUserNickname = parent.PubNickname;
            }
            _dbContext.Comments.Add(comment);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        public Page<Comment> GetCommentPage(SystemCommentPage req)
        {

            var query = _dbContext.Comments.Where(c => req.Key != null && (c.PubNickname.Contains(req.Key) ||
                                         c.ProductId.Contains(req.Key) ||
                                         c.Content.Contains(req.Key)))
                                         .OrderByDescending(c => c.CreateTime);
            var total = query.Count();
            var comments = query.Skip((req.PageNumber - 1) * req.PageSize)
                                .Take(req.PageSize).ToList();
            return new Page<Comment>
            {
                Items = comments,
                Total = total,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            };
        }

        public Result DeleteComment(string id) {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            _dbContext.Comments.Remove(comment);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        private static void ResolveCommentTree(List<CommentObj> comments)
        {
            var commentMap = comments.ToDictionary(c => c.Id);
            foreach (var comment in comments)
                if (comment.ParentId != null
                    && commentMap.TryGetValue(comment.ParentId, out var parent))
                    parent.CommentChildren ??= [comment];
        }
    }
}