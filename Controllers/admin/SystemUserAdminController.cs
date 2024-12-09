using Market.Constants;
using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,System")]
    [Route("admin/user")]
    public class SystemUserAdminController(ISystemUserService systemUserService, ITokenService tokenService) : ControllerBase {
        private readonly ISystemUserService _systemUserService = systemUserService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpGet("getUserInfo")]
        public IActionResult GetUserInfo() {
            return Ok(_systemUserService.GetUserInfo());
        }

        [HttpGet("page")]
        public IActionResult GetUserList([FromBody] SystemUserPage page) {
            return Ok(_systemUserService.GetUserList(page));
        }

        [HttpPost]
        public IActionResult Create([FromBody] SystemUserCreate dto) {
            return Ok(_systemUserService.CreateSystemUser(dto));
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] string id) {
            return Ok(_systemUserService.DeleteSystemUser(id));
        }

        [HttpPost("logout")]
        public IActionResult Logout() {
            _tokenService.LogoutCurrentUser(true);
            if (_tokenService.GetCurrentLoginUserId() == null) {
                return Ok(Result.Ok());
            } else {
                return Ok(Result.Fail(AuthCode.UserPermissionUnauthorized));
            }
        }
    }
}