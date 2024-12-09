using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("comment")]
    public class CommentController(ICommentService commentService) : ControllerBase {
        private readonly ICommentService _commentService = commentService;

        [HttpPost]
        public IActionResult CreateComment([FromBody] CreateComment dto)
        {
            return Ok(_commentService.CreateComment(dto));
        }

        [HttpGet("byProduct")]
        public IActionResult GetCommentList([FromQuery] string productId)
        {
            return Ok(_commentService.GetCommentList(productId));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment([FromRoute] string id)
        {
            return Ok(_commentService.DeleteComment(id));
        }
     }
}