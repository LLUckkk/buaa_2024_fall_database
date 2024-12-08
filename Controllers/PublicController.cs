using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("public")]
    public class PublicController(IUserService userService) : ControllerBase
    { 
        private readonly IUserService _userService = userService;

        // POST /public/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister dto)
        {
            return Ok(_userService.Register(dto));
        }

        // POST /public/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin dto)
        {
            return Ok(_userService.Login(dto));
        }
    }
}