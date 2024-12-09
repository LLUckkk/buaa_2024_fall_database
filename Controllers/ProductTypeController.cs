using Market.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("/product/type")]
    public class ProductTypeController(IProductTypeService productTypeService) : ControllerBase { 
        private readonly IProductTypeService _productTypeService = productTypeService;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productTypeService.GetList());
        }

    }
}