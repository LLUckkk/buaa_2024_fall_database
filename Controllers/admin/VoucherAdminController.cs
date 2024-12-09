using Market.Interfaces;
using Market.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("admin/product/voucher")]
    public class VoucherAdminController(IVoucherService voucherService) : ControllerBase
    {
        private readonly IVoucherService _voucherService = voucherService;

        [HttpPost]
        public IActionResult CreateVoucher([FromBody] ProductVoucherCreate dto)
        {
            return Ok(_voucherService.CreateProductVoucher(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVoucher([FromRoute] string id)
        {
            return Ok(_voucherService.DeleteProductVoucher(id));
        }
    }
}