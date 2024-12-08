using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IChatListService
    {
        Result<string> Create(ChatListCreate req);
        Result<List<ChatListObj>> GetList();
        Result<int> GetUnreadCount();
        Result<List<ChatMessage>> GetChatMessageList(string chatId);
        Result UpdateChatReadStatus(string chatId);
    }
}