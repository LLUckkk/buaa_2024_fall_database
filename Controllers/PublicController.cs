using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("public")]
    public class PublicController(IUserService userService, ISystemUserService systemUserService) : ControllerBase
    { 
        private readonly IUserService _userService = userService;
        private readonly ISystemUserService _systemUserService = systemUserService;

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister dto)
        {
            return Ok(_userService.Register(dto));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin dto)
        {
            return Ok(_userService.Login(dto));
        }

        [HttpPost("getValidateCode")]
        public IActionResult GetValidateCode([FromBody] string email) {
            return Ok(_userService.GetValidateToken(email));
        }

        [HttpPost("reset")]
        public IActionResult ResetPassword([FromBody] ResetPasswordObj dto) {
            return Ok(_userService.ResetPassword(dto));
        }

        [HttpPost("admin/user/login")]
        public IActionResult AdminLogin([FromBody] SystemUserLogin dto)
        {
            return Ok(_systemUserService.Login(dto));
        }
    }
}