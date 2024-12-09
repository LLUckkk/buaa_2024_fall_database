using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("chat")]
    public class ChatController(IChatService chatService) : ControllerBase { 
        private readonly IChatService _chatService = chatService;

        [HttpPost("list")]
        public IActionResult CreateChatList([FromBody] ChatListCreate dto)
        {
            return Ok(_chatService.Create(dto));
        }

        [HttpGet("list/all")]
        public IActionResult GetMyChatList()
        {
            return Ok(_chatService.GetList());
        }

        [HttpGet("list/one")]
        public IActionResult GetChatList([FromQuery] string chatId)
        {
            return Ok(_chatService.GetSingleList(chatId));
        }

        [HttpGet("list/unread/total")]
        public IActionResult GetTotalUnreadCount()
        {
            return Ok(_chatService.GetTotalUnreadCount());
        }

        [HttpGet("message")]
        public IActionResult GetChatMessageList([FromQuery] string chatId)
        {
            return Ok(_chatService.GetChatMessageList(chatId));
        }

        [HttpPut("message/{Id}")]
        public IActionResult UpdateChatReadStatus([FromRoute] string Id)
        {
            return Ok(_chatService.UpdateChatReadStatus(Id));
        }
    }
}