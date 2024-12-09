using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("admin/comment")]
    public class CommentAdminController(ICommentService commentService) : ControllerBase { 
        private readonly ICommentService _commentService = commentService;

        [HttpGet("page")]
        public IActionResult GetPage([FromBody]SystemCommentPage page)
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