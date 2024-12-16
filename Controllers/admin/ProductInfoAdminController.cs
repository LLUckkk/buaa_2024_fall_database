using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,System")]
    [Route("admin/product/info")]
    public class ProductInfoAdminController(IProductInfoService productInfoService) : ControllerBase {
        private readonly IProductInfoService _productInfoService = productInfoService;

        [HttpGet("page")]
        public IActionResult GetPage([FromQuery] SystemProductInfoPage page)
        {
            return Ok(_productInfoService.GetProductInfoPage(page));
        }

        [HttpPut("/down/{id}")]
        public IActionResult DownProduct(string id)
        {
            return Ok(_productInfoService.HideProduct(id));
        }
    }
}