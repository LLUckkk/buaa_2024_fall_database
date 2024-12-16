using Market.Entities;
using Market.Models;

namespace Market.Interfaces
{
    public interface IChatService
    {
        ChatList? GetChatList(string chatId);
        Result<string> Create(ChatListCreate req);
        Result<List<ChatListObj>> GetList();
        Result<ChatListObj> GetSingleList(string chatId);
        Result<string> SendMessageSimple(ChatMessageWsObj req);
        Result<int> GetTotalUnreadCount();
        Result<List<ChatMessage>> GetChatMessageList(string chatId);
        Result UpdateChatReadStatus(string chatId);
    }
}