using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        // GET /user/logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        // GET /user/getUserInfo
        [HttpGet("getUserInfo")]
        public IActionResult GetUserInfo()
        {
            return Ok();
        }

        // GET /user/getUserInfo/byId
        [HttpGet("getUserInfo/byId")]
        public IActionResult GetUserInfoById(string userId)
        {
            return Ok();
        }

        // PUT /user
        [HttpPut]
        public IActionResult UpdateUserInfo([FromBody] UpdateUserInfo dto)
        {
            return Ok();
        }

        // PUT /user/password
        [HttpPut("password")]
        public IActionResult UpdateUserInfoPass([FromBody] UpdateUserInfo dto)
        {
            return Ok();
        }
    }
}