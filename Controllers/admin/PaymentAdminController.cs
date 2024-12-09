using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("admin/payment")]
    public class PaymentAdminController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpGet("order/page")]
        public IActionResult GetOrderPage([FromBody] SystemPaymentOrderPage page)
        {
            return Ok(_paymentService.GetPaymentOrderPage(page));
        }

        [HttpGet("order/detail/{id}")]
        public IActionResult GetOrderDetail([FromRoute] string id)
        {
            return Ok(_paymentService.GetPaymentOrderDetail(id));
        }
    }
}