using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("admin/product/type")]
    [Authorize(Roles = "Admin,System")]
    public class ProductTypeAdminController(IProductTypeService productTypeService) : ControllerBase {
        private readonly IProductTypeService _productTypeService = productTypeService;

        [HttpGet("page")]
        public IActionResult GetTypePage([FromQuery] SystemProductTypePage dto) {
            return Ok(_productTypeService.GetProductTypeList(dto));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductTypeObj dto) {
            return Ok(_productTypeService.CreateProductType(dto));
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProductTypeObj dto) {
            return Ok(_productTypeService.UpdateProductType(dto));
        }
    }
}