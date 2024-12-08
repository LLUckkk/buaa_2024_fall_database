using Market.Models;

namespace Market.Interfaces
{
    public interface IChatListService
    {
        string Create(ChatListCreate req);
        List<ChatList> GetList();
        int GetUnreadCount();
    }
}