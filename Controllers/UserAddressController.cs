using Market.Entities;
using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("user/address")]
    public class UserAddressController(IUserAddressService userAddressService) : ControllerBase
    {
        private readonly IUserAddressService _userAddressService = userAddressService;
    
        [HttpPost]
        public IActionResult Create([FromBody] UserAddressObj address)
        {
            return Ok(_userAddressService.AddUserAddress(address));
        }

        // TODO : Implement Update method
        [HttpPut]
        public IActionResult Update([FromBody] UserAddressObj address)
        {
            return Ok(_userAddressService.UpdateUserAddress(address));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_userAddressService.DeleteUserAddress(id));
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            return Ok(_userAddressService.GetUserAddressList());
        }
    }
}