using Market.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController(IPaymentService paymentService) : ControllerBase {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpPost("pay")]
        public IActionResult Pay()
        {
            return Ok();
        }
     }
}