using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(IUserService userService, ITokenService tokenService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        // GET /user/logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _tokenService.LogoutCurrentUser();
            return Ok();
        }

        // GET /user/getUserInfo
        [HttpGet("getUserInfo")]
        public IActionResult GetUserInfo()
        {
            return Ok(_userService.GetUser());
        }

        // GET /user/getUserInfo/byId
        [HttpGet("getUserInfo/byId")]
        public IActionResult GetUserInfoById(string userId)
        {
            return Ok(_userService.GetUserById(userId));
        }

        // PUT /user
        [HttpPut]
        public IActionResult UpdateUserInfo([FromBody] UpdateUserInfo dto)
        {
            return Ok(_userService.UpdateUserInfo(dto));
        }

        // PUT /user/password
        [HttpPut("password")]
        public IActionResult UpdateUserInfoPass([FromBody] UpdateUserInfo dto)
        {
            return Ok(_userService.UpdateUserPassword(dto));
        }
    }
}