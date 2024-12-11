using Market.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    [Route("/voucher/order")]
    public class VoucherOrderController(IVoucherService voucherService) : ControllerBase
    {
        private readonly IVoucherService _voucherService = voucherService;

        [HttpPost("seckill/{id}")]
        public IActionResult Seckill([FromRoute] string id)
        {
            return Ok(_voucherService.CreateTimeLimitedOfferOrder(id));
        }
    }
}