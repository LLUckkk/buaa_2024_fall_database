using Market.Interfaces;
using Market.Models;

namespace Market.Services {
    // TODO
    public class ChatListService(IUserService userService) : IChatListService {
        private readonly IUserService _userService = userService;
        public string Create(ChatListCreate req) {
            // TODO
            return "";
        }
        public List<ChatList> GetList() {
            return new List<ChatList>();
        }
        public int GetUnreadCount() {
            return 0;
        }
    }
}