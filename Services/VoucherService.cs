using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Services
{
    public class VoucherService(ITokenService tokenService, ApplicationDbContext dbcontext) : IVoucherService
    {
        private readonly ApplicationDbContext _dbContext = dbcontext;
        private readonly ITokenService _tokenService = tokenService;

        public Result CreateProductVoucher(ProductVoucherCreate req)
        {
            var existing = _dbContext.ProductVouchers.FirstOrDefault(pv => pv.ProductId == req.ProductId);
            if (existing != null)
            {
                existing.Title = "下单立减 " + req.VoucherValue + " 元";
                existing.Stock = 1;
                existing.VoucherValue = (long)(100 * req.VoucherValue);
                existing.BeginTime = DateTimeOffset.FromUnixTimeSeconds(req.BeginTime).DateTime;
                existing.EndTime = DateTimeOffset.FromUnixTimeSeconds(req.EndTime).DateTime;
                existing.UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                return Result.Ok();
            }
            else
            {
                var voucher = new ProductVoucher
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = req.ProductId,
                    Title = "下单立减 " + req.VoucherValue + " 元",
                    VoucherValue = (long)(100 * req.VoucherValue),
                    Stock = 1,
                    BeginTime = DateTimeOffset.FromUnixTimeSeconds(req.BeginTime).DateTime,
                    EndTime = DateTimeOffset.FromUnixTimeSeconds(req.EndTime).DateTime,
                    VoucherStatus = 9,
                    CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                _dbContext.ProductVouchers.Add(voucher);
                var save = _dbContext.SaveChanges();
                if (save == 0)
                    return Result.Fail(ResultCode.SaveError);
                return Result.Ok();
            }
        }
        public Result CreateTimeLimitedOfferOrder(string voucherId)
        {
            var userid = _tokenService.GetCurrentLoginUserId();
            var voucher = _dbContext.ProductVouchers.FirstOrDefault(pv => pv.Id == voucherId);
            if (voucher == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            var now = DateTimeOffset.Now;
            if (now < voucher.BeginTime || now > voucher.EndTime)
            {
                return Result.Fail(ResultCode.Fail, now < voucher.BeginTime ? "活动未开始" : "活动已结束");
            }
            var count = _dbContext.VoucherOrders.Count(pvo => pvo.UserId == userid && pvo.VoucherId == voucherId);
            if (count > 0)
            {
                return Result.Fail(ResultCode.Fail, "已下单成功，请勿重复下单");
            }
            var success = _dbContext.Database.ExecuteSqlRaw("update product_voucher set stock = stock - 1 where id = {0} and stock > 0", voucherId);
            if (success == 0)
            {
                return Result.Fail(ResultCode.Fail, "库存不足");
            }
            var order = new VoucherOrder
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userid,
                VoucherId = voucherId,
                Status = 9,
                CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            _dbContext.VoucherOrders.Add(order);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        public Result<ProductVoucher?> GetProductVoucher(string id)
        {
            var voucher = _dbContext.ProductVouchers.FirstOrDefault(pv => pv.Id == id);
            if (voucher == null)
            {
                return Result<ProductVoucher?>.Fail(ResultCode.NotFoundError);
            }
            return Result<ProductVoucher?>.Ok(voucher);
        }

        public Result DeleteProductVoucher(string id)
        {
            var voucher = _dbContext.ProductVouchers.FirstOrDefault(pv => pv.Id == id);
            if (voucher == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            _dbContext.ProductVouchers.Remove(voucher);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }
    }
}