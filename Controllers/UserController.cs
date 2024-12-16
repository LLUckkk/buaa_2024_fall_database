using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("user")]
    public class UserController(IUserService userService, ITokenService tokenService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        // GET /user/logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _tokenService.LogoutCurrentUser(false);
            return Ok(Result.Ok());
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
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return Ok(Result<User?>.Fail(ResultCode.NotFoundError));
            }
            return Ok(Result<User?>.Ok(user));
            // return Ok(Result<User?>.Ok(_userService.GetUserById(userId)));
        }

        // // GET /user/websocketToken
        // [HttpGet("websocketToken")]
        // public IActionResult GetOneTimeWebsocketToken()
        // {
        //     return Ok(_userService.GetOneTimeWebsocketToken());
        // }

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