using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("admin/web/user")]
    public class UserAdminController(IUserService userService) : ControllerBase { 
        private readonly IUserService _userService = userService;

        [HttpGet("page")]
        public IActionResult GetUserList([FromQuery] UserAdminPage page) {
            return Ok(_userService.getUserList(page));
        }

        [HttpPut("able/{id}")]
        public IActionResult ApproveUser([FromRoute] string id) {
            return Ok(_userService.EnableUser(id));
        }

        [HttpPut("disable/{id}")]
        public IActionResult DisableUser([FromRoute] string id) {
            return Ok(_userService.DisableUser(id));
        }

        [HttpPut("check/success/{id}")]
        public IActionResult ApproveProfileUpdate([FromRoute] string id) {
            return Ok(_userService.ApproveUserProfileUpdate(id));
        }

        [HttpPut("check/fail/{id}")]
        public IActionResult RejectProfileUpdate([FromRoute] string id) {
            return Ok(_userService.RejectUserProfileUpdate(id));
        }
    }
}