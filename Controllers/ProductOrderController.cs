using Market.Interfaces;
using Market.Models;
using Market.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("product/order")]
    public class ProductOrderController(IProductOrderService productOrderService) : ControllerBase
    {
        private readonly IProductOrderService _productOrderService = productOrderService;
    
        [HttpPost]
        public IActionResult CreateOrder([FromBody] ProductOrderObj dto) {
            return Ok(_productOrderService.CreateProductOrder(dto.ProductId, dto.Info, dto.Address, dto.Idname, dto.Phone));
        }

        [HttpGet]
        public IActionResult GetProductOrderInfo([FromQuery] string orderId) {
            return Ok(_productOrderService.GetProductOrderDetail(orderId));
        }

        [HttpGet("detail")]
        public IActionResult GetOrderDetail([FromQuery] string orderId) {
            return Ok(_productOrderService.GetProductOrderDetail(orderId));
        }

        [HttpGet("/my/sell")]
        public IActionResult GetMySellOrderList() {
            return Ok(_productOrderService.GetMySellOrderList());
        }

        [HttpGet("/my/buy")]
        public IActionResult GetMyBuyOrderList() {
            return Ok(_productOrderService.GetMyBuyOrderList());
        }

        [HttpPut("user/self/{productOrderId}")]
        public IActionResult UserSelfPickup(string productOrderId) {
            return Ok(_productOrderService.UserPerformSelfPickup(productOrderId));
        }

        [HttpPut("post")]
        public IActionResult UserPerformDelivery([FromBody] ProductOrderPost dto) {
            return Ok(_productOrderService.UserPerformDelivery(dto));
        }

        [HttpPut("selfpost")]
        public IActionResult UserConfigurePickupAddress([FromBody] ProductOrderPost dto) {
            return Ok(_productOrderService.UserConfigurePickupAddress(dto));
        }

        [HttpPut("user/confirm/{productOrderId}")]
        public IActionResult UserConfirmDelivery(string productOrderId) {
            return Ok(_productOrderService.UserConfirmDelivery(productOrderId));
        }

        [HttpPut("evaluate")]
        public IActionResult UserFeedback([FromBody] ProductOrderEvaluate dto) {
            return Ok(_productOrderService.UserFeedback(dto));
        }
    }
}