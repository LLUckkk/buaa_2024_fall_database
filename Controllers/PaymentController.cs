using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController(IPaymentService paymentService) : ControllerBase {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpPost("order")]
        public IActionResult CreatePaymentOrder([FromBody]PaymentOrderObj dto)
        {
            return Ok(_paymentService.CreatePaymentOrder(dto.OrderId));
        }

        [HttpGet("order")]
        public IActionResult GetPaymentOrder([FromQuery] string orderId)
        {
            return Ok(_paymentService.GetPaymentOrderById(orderId));
        }

        [HttpPost("pay")]
        public IActionResult CreatePay([FromBody] PaymentPayObj dto) 
        {
            return Ok(_paymentService.CreatePay(dto.PaymentOrderId));
        }

        [HttpPut("finish/{payId}")]
        public IActionResult FinishPay(string payId)
        {
            return Ok(_paymentService.FinishPay(payId));
        }
     }
}