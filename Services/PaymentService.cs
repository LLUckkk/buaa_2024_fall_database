using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class PaymentService(ApplicationDbContext dbContext, IUserService userService, IProductOrderService productOrderService) : IPaymentService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;
        private readonly IProductOrderService _productOrderService = productOrderService;
        public Result<string> CreatePaymentOrder(string productId)
        {
            var user = _userService.GetCurrentUser();
            var productOrder = _dbContext.ProductOrders.FirstOrDefault(o => o.ProductId == productId && o.UserId == user.Id);
            if (productOrder == null || productOrder.DealStatus != 0)
                return Result<string>.Fail(ResultCode.ValidateError);
            if (productOrder.Price <= 0)
            {
                return Result<string>.Fail(ResultCode.Fail);
            }
            var paymentType = _dbContext.PaymentTypes.FirstOrDefault(p => p.Id == 1);
            var paymentOrder = new PaymentOrder
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                UserId = user.Id,
                PayPrice = productOrder.Price,
                PayTypeId = 1,
                PayTypeName = paymentType!.TypeName,
                OrderStatus = 0,
                PaymentStatus = 0,
                ProcessStatus = 0,
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
            };
            _dbContext.PaymentOrders.Add(paymentOrder);
            productOrder.PayOrderId = paymentOrder.Id;
            _dbContext.ProductOrders.Update(productOrder);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result<string>.Fail(ResultCode.SaveError);
            return Result<string>.Ok(paymentOrder.Id);
        }
        public void PaymentOrderStatusUpdateCallback(PaymentOrder order)
        {
            if (order.PayTypeId == 1)
            {
                order.PaymentStatus = 9;
                order.TimeFinish = DateTime.UtcNow;
                _dbContext.PaymentOrders.Update(order);
                _dbContext.ProductOrders.Where(o => o.PayOrderId == order.Id).ToList().ForEach(o =>
                {
                    o.DealStatus = 3;
                    o.PayStatus = 9;
                    _dbContext.ProductOrders.Update(o);
                });
            }
            _dbContext.SaveChanges();
            // Else?
        }
        public Result<Page<PaymentOrder>> GetPaymentOrderPage(SystemPaymentOrderPage req)
        {
            var orders = _dbContext.PaymentOrders.Where(o => req.Key != null && o.OrderNumber.Contains(req.Key))
                    .OrderByDescending(o => o.TimeCreate)
                    .Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();
            return Result<Page<PaymentOrder>>.Ok(new Page<PaymentOrder>
            {
                Items = orders,
                Total = orders.Count,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            });
        }

        public Result<PaymentOrderAdminDetail> GetPaymentOrderDetail(string orderId) {
            var order = _dbContext.PaymentOrders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return Result<PaymentOrderAdminDetail>.Fail(ResultCode.NotFoundError);
            }
            var user = _userService.GetUserById(order.UserId);
            if (user == null)
            {
                return Result<PaymentOrderAdminDetail>.Fail(ResultCode.NotFoundError);
            }
            return Result<PaymentOrderAdminDetail>.Ok(new PaymentOrderAdminDetail
            {
                PaymentOrder = order,
                User = user,
            });
        }

        public Result<string> CreatePay(string orderId) {
            var order = _dbContext.PaymentOrders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return Result<string>.Fail(ResultCode.NotFoundError);
            }
            if (order.OrderStatus > 0) {
                return Result<string>.Fail(ResultCode.Fail, "订单已锁定");
            }
            if (!order.UserId.Equals(_userService.GetCurrentUser()?.Id))
            {
                return Result<string>.Fail(ResultCode.Fail);
            }
            if (order.PaymentStatus != 0 || order.PayPrice <= 0)
            {
                return Result<string>.Fail(ResultCode.Fail);
            }
            if (order.TimeCreate.AddMinutes(5) < DateTime.UtcNow)
            {
                return Result<string>.Fail(ResultCode.Fail, "订单已过期");
            }
            var type = _dbContext.PaymentTypes.FirstOrDefault(t => t.Id == order.PayTypeId)!;
            var pay = new PaymentPay
            {
                Id = Guid.NewGuid().ToString(),
                UserId = order.UserId,
                OrderId = order.Id,
                PaymentPrice = (long)(order.PayPrice * type.WxPay),
                PaymentType = "weixin",
                PaymentTimeStart = DateTime.UtcNow.ToString("yyyyMMssHHmmss"),
                PaymentTimeExpire = DateTime.UtcNow.AddMinutes(5).ToString("yyyyMMssHHmmss"),
                TimeCreate = DateTime.UtcNow,
                TimeUpdate = DateTime.UtcNow,
            };
            _dbContext.PaymentPays.Add(pay);
            order.PaymentStatus = 1;
            order.PaymentPayId = pay.Id;
            order.TimeUpdate = DateTime.UtcNow;
            _dbContext.PaymentOrders.Update(order);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result<string>.Fail(ResultCode.SaveError);
            return Result<string>.Ok(pay.Id);
        }
        public Result FinishPay(string payId) {
            var pay = _dbContext.PaymentPays.FirstOrDefault(p => p.Id == payId);
            if (pay == null)
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            var order = _dbContext.PaymentOrders.FirstOrDefault(o => o.Id == pay.OrderId);
            if (order == null)
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            if (pay.PaymentStatus == 9 || pay.PaymentStatus == 8)
            {
                return Result.Fail(ResultCode.Fail, "订单已支付");
            }
            pay.PaymentResultData = "支付成功";
            pay.PaymentStatus = 9;
            pay.ProcessStatus = 1;
            pay.TimeUpdate = DateTime.UtcNow;
            _dbContext.PaymentPays.Update(pay);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();        }
        public void PayStatusUpdateCallback(PaymentPay pay) {
            pay.ProcessStatus = 9;
            pay.TimeFinish = DateTime.UtcNow;
            _dbContext.PaymentPays.Update(pay);

            var order = _dbContext.PaymentOrders.FirstOrDefault(o => o.Id == pay.OrderId);
            if (order == null)
            {
                return;
            }
            order.PaymentStatus = 9;
            order.OrderStatus = 2;
            order.ProcessStatus = 1;
            order.TimeFinish = DateTime.UtcNow;
            order.PaymentType = pay.PaymentType;
            _dbContext.PaymentOrders.Update(order);
            _dbContext.SaveChanges();
        }

        public Result<PaymentOrder> GetPaymentOrderById(string orderId) {  
            var order = _dbContext.PaymentOrders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return Result<PaymentOrder>.Fail(ResultCode.NotFoundError);
            }
            return Result<PaymentOrder>.Ok(order);
        }
    }
}