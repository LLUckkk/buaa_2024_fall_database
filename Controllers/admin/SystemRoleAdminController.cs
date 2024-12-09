using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "System")]
    [Route("admin/role")]
    public class SystemRoleAdminController(ISystemUserService systemUserService) : ControllerBase { 
        private readonly ISystemUserService _systemUserService = systemUserService;

        [HttpPost]
        public IActionResult Create([FromBody] SystemRoleCreate dto)
        {
            return Ok(_systemUserService.CreateRole(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            return Ok(_systemUserService.DeleteRole(id));
        }

        [HttpPut]
        public IActionResult Update([FromBody] SystemRoleUpdate dto)
        {
            return Ok(_systemUserService.UpdateRole(dto));
        }

        [HttpGet("page")]
        public IActionResult GetPage([FromBody] SystemRolePage page)
        {
            return Ok(_systemUserService.GetRolePage(page));
        }

        [HttpGet("list")]
        public IActionResult GetList()
        {
            return Ok(_systemUserService.GetRoleList());
        }
    }
}