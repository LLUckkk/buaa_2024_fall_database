using Market.Entities;
using Market.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("user/collect")]
    public class ProductCollectController(IProductCollectService productCollectService) : ControllerBase { 
        private readonly IProductCollectService _productCollectService = productCollectService;

        [HttpPost]
        public IActionResult Create([FromBody] ProductCollect req)
        {
            return Ok(_productCollectService.Create(req));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            return Ok(_productCollectService.Delete(id));
        }

        [HttpGet("list")]
        public IActionResult GetCollectionProductIdList()
        {
            return Ok(_productCollectService.GetCollectionProductIdList());
        }
    }
}