using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("admin/comment")]
    public class CommentAdminController(ICommentService commentService) : ControllerBase { 
        private readonly ICommentService _commentService = commentService;

        [HttpGet("page")]
        public IActionResult GetPage([FromQuery]SystemCommentPage page)
        {
            return Ok(_commentService.GetCommentPage(page));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            return Ok(_commentService.DeleteComment(id));
        }
    }
}