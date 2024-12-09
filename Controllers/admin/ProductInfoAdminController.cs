using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("admin/product/info")]
    public class ProductInfoAdminController(IProductInfoService productInfoService) : ControllerBase {
        private readonly IProductInfoService _productInfoService = productInfoService;

        [HttpGet("page")]
        public IActionResult GetPage([FromBody] SystemProductInfoPage page)
        {
            return Ok(_productInfoService.GetProductInfoPage(page));
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetDetail([FromRoute] string id)
        {
            return Ok(_productInfoService.GetProductInfoAdminDetail(id));
        }

        [HttpPut("/pass/{id}")]
        public IActionResult ApproveProduct(string id)
        {
            return Ok(_productInfoService.ApproveProduct(id));
        }

        [HttpPut("/fail/{id}")]
        public IActionResult RejectProduct(string id)
        {
            return Ok(_productInfoService.RejectProduct(id));
        }

        [HttpPut("/down/{id}")]
        public IActionResult DownProduct(string id)
        {
            return Ok(_productInfoService.HideProduct(id));
        }
    }
}