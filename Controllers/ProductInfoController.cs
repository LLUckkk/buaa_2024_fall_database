using Market.Constants;
using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("product/info")]
    public class ProductInfoController(IProductInfoService productInfoService, IProductOrderService productOrderService, IVoucherService voucherService) : ControllerBase
    {
        private readonly IProductInfoService _productInfoService = productInfoService;
        private readonly IProductOrderService _productOrderService = productOrderService;
        private readonly IVoucherService _voucherService = voucherService;

        [HttpGet("page")]
        public IActionResult GetProductList([FromQuery] ProductInfoPage req)
        {
            return Ok(_productInfoService.GetProductInfoList(req));
        }

        [HttpGet("detail")]
        public IActionResult GetProductDetail([FromQuery] string id)
        {
            return Ok(_productInfoService.GetProductInfo(id));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductInfoObj product)
        {
            return Ok(_productInfoService.CreateProductInfo(product));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct([FromRoute] string id)
        {
            return Ok(_productInfoService.RemoveById(id));
        }

        [HttpPut("disable/{id}")]
        public IActionResult HideProduct([FromRoute] string id)
        {
            return Ok(_productInfoService.HideProduct(id));
        }

        [HttpPut("enable/{id}")]
        public IActionResult ShowProduct([FromRoute] string id)
        {
            return Ok(_productInfoService.ShowProduct(id));
        }

        [HttpGet("my")]
        public IActionResult GetMyProductList()
        {
            return Ok(_productInfoService.GetMyProductInfoList());
        }

        [HttpGet("/my/collect")]
        public IActionResult GetMyCollectionList()
        {
            return Ok(_productInfoService.GetMyProductCollectionInfoList());
        }

        [HttpGet("/list/my")]
        public IActionResult GetMyProductInfoList()
        {
            var publishR = _productInfoService.GetMyProductInfoList();
            var buyOrder = _productOrderService.GetMyBuyOrderList();
            var sellOrder = _productOrderService.GetMySellOrderList();
            if (publishR.Code != 1 || buyOrder.Code != 1 || sellOrder.Code != 1)
            {
                return Ok(Result.Fail(ResultCode.Fail));
            }
            var publish = publishR.Data!.Select(p =>
                new ProductInfoDetail
                {
                    Id = p.Id,
                    Title = p.Title,
                    Intro = p.Intro,
                    Image = p.Image,
                    Price = p.Price,
                    OriginalPrice = p.OriginalPrice,
                    PostType = p.PostType,
                    Adcode = p.Adcode,
                    Province = p.Province,
                    City = p.City,
                    District = p.District,
                    Status = p.Status,
                    UserId = p.UserId,
                    CreateTime = p.CreateTime,
                    UpdateTime = p.UpdateTime,
                    Type = "publish"
                }
            ).ToList();
            var buy = buyOrder.Data!.Select(
                p => _productInfoService.GetProductInfo(p.ProductId).Data!
            ).Select(p =>
                {
                    p.Type = "buy";
                    return p;
                }
            ).ToList();
            var sell = sellOrder.Data!.Select(
                p => _productInfoService.GetProductInfo(p.ProductId).Data!
            ).Select(p =>
                {
                    p.Type = "sell";
                    return p;
                }
            ).ToList();
            publish.AddRange(buy);
            publish.AddRange(sell);
            return Ok(Result<List<ProductInfoDetail>>.Ok(publish));
        }

        [HttpPut("like/count/{productId}")]
        public IActionResult LikeProduct([FromRoute] string productId)
        {
            return Ok(_productInfoService.CreateLike(productId));
        }
    }
}