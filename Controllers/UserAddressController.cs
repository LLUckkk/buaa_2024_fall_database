using Market.Entities;
using Market.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("user/address")]
    public class UserAddressController(IUserAddressService userAddressService) : ControllerBase
    {
        private readonly IUserAddressService _userAddressService = userAddressService;
    
        [HttpPost]
        public IActionResult Create([FromBody] UserAddress address)
        {
            return Ok(_userAddressService.AddUserAddress(address));
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