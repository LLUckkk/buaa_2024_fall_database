using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ChatService(ApplicationDbContext dbContext, IUserService userService) : IChatService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public Result<string> Create(ChatListCreate req)
        {
            var user = _userService.GetCurrentUser();
            var existing = _dbContext.ChatLists.FirstOrDefault(c => c.FromUserId == user.Id && c.ToUserId == req.ToUserId || c.FromUserId == req.ToUserId && c.ToUserId == user.Id);
            if (existing != null)
            {
                return Result<string>.Ok(existing.Id);
            }
            var target = _userService.GetUserById(req.ToUserId);
            if (target == null)
            {
                return Result<string>.Fail(ResultCode.NotFoundError);
            }
            var chatList = new ChatList
            {
                Id = Guid.NewGuid().ToString(),
                FromUserId = user.Id,
                ToUserId = req.ToUserId,
                CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                FromUserNickname = user.Nickname,
                ToUserNickname = target.Nickname,
                FromUserAvatar = user.Avatar,
                ToUserAvatar = target.Avatar,
                ProductId = req.ProductId,
                ProductImage = req.ProductImage,
            };
            _dbContext.ChatLists.Add(chatList);

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                ChatListId = chatList.Id,
                FromUserId = user.Id,
                FromUserNick = user.Nickname,
                ToUserNick = target.Nickname,
                ToUserId = req.ToUserId,
                Content = "我刚刚查看了您的宝贝哦～",
                SendTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                IsRead = 0,
            };
            _dbContext.ChatMessages.Add(chatMessage); 
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result<string>.Fail(ResultCode.SaveError);
            return Result<string>.Ok(chatList.Id);
        }
        public Result<List<ChatListObj>> GetList()
        {
            var user = _userService.GetCurrentUser();
            var list = _dbContext.ChatLists
                        .Where(c => c.FromUserId == user.Id || c.ToUserId == user.Id).OrderByDescending(c => c.UpdateTime)
                        .Select(c => ChatListObj.FromChatList(c))
                        .ToList()
                        .Select(obj =>
                        {
                            obj.ChatMessage = GetListLastMessage(obj.Id);
                            obj.NoReadCount = GetListUnreadCount(obj.Id);
                            return obj;
                        })
                        .ToList();
            return Result<List<ChatListObj>>.Ok(list);
        }

        public Result<ChatListObj> GetSingleList(string chatId)
        {
            var user = _userService.GetCurrentUser();
            var chat = _dbContext.ChatLists.FirstOrDefault(c => c.Id == chatId);
            if (chat == null)
            {
                return Result<ChatListObj>.Fail(ResultCode.NotFoundError);
            }
            var obj = ChatListObj.FromChatList(chat);
            obj.ChatMessage = GetListLastMessage(chatId);
            obj.NoReadCount = GetListUnreadCount(chatId);
            return Result<ChatListObj>.Ok(obj);
        }

        public Result<int> GetTotalUnreadCount()
        {
            var user = _userService.GetCurrentUser();
            var count = _dbContext.ChatLists
                        .Where(c => c.FromUserId == user.Id || c.ToUserId == user.Id)
                        .ToList()
                        .AsEnumerable()
                        .Sum(c => GetListUnreadCount(c.Id));
            return Result<int>.Ok(count);
        }

        public Result<List<ChatMessage>> GetChatMessageList(string chatId)
        {
            var user = _userService.GetCurrentUser();
            var list = _dbContext.ChatMessages
                        .Where(m => m.ChatListId == chatId)
                        .OrderBy(m => m.SendTime)
                        .ToList();
            return Result<List<ChatMessage>>.Ok(list);
        }
        public Result UpdateChatReadStatus(string chatId)
        {
            var user = _userService.GetCurrentUser();
            var list = _dbContext.ChatMessages
                        .Where(m => m.ChatListId == chatId && m.ToUserId == user.Id && m.IsRead == 0)
                        .ToList();
            list.ForEach(m => m.IsRead = 1);
            _dbContext.ChatMessages.UpdateRange(list);
            return Result.Ok();
        }

        private ChatMessage GetListLastMessage(string chatId)
        {
            var message = _dbContext.ChatMessages
                        .Where(m => m.ChatListId == chatId)
                        .OrderByDescending(m => m.SendTime)
                        .FirstOrDefault();
            if (message == null)
            {
                return new ChatMessage
                {
                    Id = "",
                    ChatListId = chatId,
                    FromUserId = "",
                    ToUserId = "",
                    FromUserNick = "",
                    ToUserNick = "",
                    Content = "暂无消息",
                    SendTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                };
            }
            return message;
        }

        private int GetListUnreadCount(string chatId)
        {
            var user = _userService.GetCurrentUser()!;
            var count = _dbContext.ChatMessages
                        .Where(m => m.ChatListId == chatId && m.ToUserId == user.Id && m.IsRead == 0)
                        .Count();
            return count;
        }
    }
}