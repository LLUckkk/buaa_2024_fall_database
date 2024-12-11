using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductOrderService(ApplicationDbContext dbContext, IUserService userService) : IProductOrderService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public Result<string> CreateProductOrder(string productId, string info, string address, string name, string phone)
        {
            var user = _userService.GetCurrentUser();
            var product = _dbContext.ProductInfos.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return Result<string>.Fail(ResultCode.NotFoundError);
            }
            if (product.Status != 9)
            {
                return Result<string>.Fail(ResultCode.BusinessError, "商品目前不可购买");
            }
            if (product.UserId == user.Id)
            {
                return Result<string>.Fail(ResultCode.BusinessError, "不能购买自己的商品");
            }
            var po = new ProductOrder
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                ProductId = productId,
                ProductUserId = product.UserId,
                UserId = user.Id,
                ProductType = product.TypeCode,
                ProductTypeName = product.TypeName,
                ProductTitle = product.Title,
                ProductImg = product.Image,
                ProductPrice = product.OriginalPrice,
                ProductSellPrice = product.Price,
                ProductNum = 1,
                ProductPost = 0,
                ProductPostStatus = product.PostType,
                ProductInfo = info,
                ProductMoney = product.Price,
                PostMode = product.PostType == 1 ? "自提" : "快递",
                PostUsername = name,
                PostPhone = phone,
                PostAddress = product.PostType == 1 ? null : address,
                DealStatus = 0,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
            };
            var VoucherOrder = _dbContext.VoucherOrders.FirstOrDefault(v => v.UserId == user.Id && v.ProductId == productId && v.Status == 9);
            if (VoucherOrder != null)
            {
                po.Price -= VoucherOrder.VoucherValue;
                VoucherOrder.Status = 0;
                _dbContext.VoucherOrders.Update(VoucherOrder);
            }
            if (po.Price <= 0)
            {
                po.Price = 0;
            }
            po.BuyMoneyAll = po.Price;
            po.BuyInfo = info;
            _dbContext.ProductOrders.Add(po);
            product.Status = 12;
            product.UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            _dbContext.ProductInfos.Update(product);
            var collect = _dbContext.ProductCollects.FirstOrDefault(c => c.ProductId == productId && c.UserId == user.Id);
            if (collect != null)
            {
                _dbContext.ProductCollects.Remove(collect);
            }
                        var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result<string>.Fail(ResultCode.SaveError);
            return Result<string>.Ok(po.Id);
        }
        public void ClearOutdatedOrder(ProductOrder order)
        {
            order.PostSelfCode = "";
            order.DealStatus = 1;
            order.UpdateTime = DateTime.UtcNow;
            _dbContext.ProductOrders.Update(order);
        }
        public Result<List<ProductOrder>> GetMySellOrderList()
        {
            var user = _userService.GetCurrentUser();
            var orders = _dbContext.ProductOrders.Where(o => o.ProductUserId == user.Id).OrderByDescending(o => o.CreateTime).ToList();
            return Result<List<ProductOrder>>.Ok(orders);
        }
        public Result<List<ProductOrder>> GetMyBuyOrderList()
        {
            var user = _userService.GetCurrentUser();
            var orders = _dbContext.ProductOrders.Where(o => o.UserId == user.Id).OrderByDescending(o => o.CreateTime).ToList();
            return Result<List<ProductOrder>>.Ok(orders);
        }
        public Result<Page<ProductOrder>> GetProductOrderList(SystemProductOrderPage req)
        {
            var orders = _dbContext.ProductOrders.Where(o => req.Key != null && (o.OrderNumber.Contains(req.Key) || o.ProductInfo.Contains(req.Key)))
                .OrderByDescending(o => o.CreateTime)
                .Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();
            return Result<Page<ProductOrder>>.Ok(new Page<ProductOrder>
            {
                Items = orders,
                Total = orders.Count,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            });
        }
        public Result<ProductOrderDetail> GetProductOrderDetail(string orderId) {
            var order = _dbContext.ProductOrders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return Result<ProductOrderDetail>.Fail(ResultCode.NotFoundError);
            }
            var product = _dbContext.ProductInfos.FirstOrDefault(p => p.Id == order.ProductId);
            if (product == null)
            {
                return Result<ProductOrderDetail>.Fail(ResultCode.NotFoundError);
            }
            var paymentOrder = _dbContext.PaymentOrders.FirstOrDefault(p => p.Id == order.PayOrderId);
            if (paymentOrder == null)
            {
                return Result<ProductOrderDetail>.Fail(ResultCode.NotFoundError);
            }
            var paymentPay = _dbContext.PaymentPays.FirstOrDefault(p => p.OrderId == order.PayOrderId);
            if (paymentPay == null)
            {
                return Result<ProductOrderDetail>.Fail(ResultCode.NotFoundError);
            }
            var detail = new ProductOrderDetail
            {
                ProductOrder = order,
                ProductInfo = product,
                PaymentOrder = paymentOrder,
                PaymentPay = paymentPay,
            };
            return Result<ProductOrderDetail>.Ok(detail);
        }
        public Result<Page<ProductOrder>> GetProductOrderToBeApprovedList(SystemProductOrderPage req) {
            var orders = _dbContext.ProductOrders
                .Where(o => req.Key != null && (o.OrderNumber.Contains(req.Key) || o.ProductInfo.Contains(req.Key)) && o.DealStatus == 11)
                .OrderByDescending(o => o.CreateTime)
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();

            return Result<Page<ProductOrder>>.Ok(new Page<ProductOrder>
            {
                Items = orders,
                Total = orders.Count,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            });
        }

        public Result<long> GetTodayCount() {
            var startDay = DateTime.Today;
            var endDay = DateTime.UtcNow;
            var count = _dbContext.ProductOrders.Count(o => o.CreateTime >= startDay && o.CreateTime <= endDay);
            return Result<long>.Ok(count);
        }
        public Result<long> GetMonthCount() {
            var startDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endDay = DateTime.UtcNow;
            var count = _dbContext.ProductOrders.Count(o => o.CreateTime >= startDay && o.CreateTime <= endDay);
            return Result<long>.Ok(count);
        }
        public Result<long> GetTodayTurnover() {
            long money = 0L;
            var startDay = DateTime.Today;
            var endDay = DateTime.UtcNow;

            var list = _dbContext.ProductOrders.Where(o => o.CreateTime >= startDay && o.CreateTime <= endDay).ToList();
            foreach (var productOrder in list)
            {
                money += productOrder.BuyMoneyAll;
            }
            return Result<long>.Ok(money);
        }
        public Result<long> GetMonthTurnover() {
            long money = 0L;
            var startDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endDay = DateTime.UtcNow;

            var list = _dbContext.ProductOrders.Where(o => o.CreateTime >= startDay && o.CreateTime <= endDay).ToList();
            foreach (var productOrder in list)
            {
                money += productOrder.BuyMoneyAll;
            }
            return Result<long>.Ok(money);
        }

        public Result UserPerformSelfPickup(string orderId) {
            var order = _dbContext.ProductOrders.FirstOrDefault(o => o.Id == orderId && o.DealStatus == 4 && o.PostMode == "自提");
            if (order == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            order.DealStatus = 9;
            _dbContext.ProductOrders.Update(order);
            return Result.Ok();
        }
        public Result UserPerformDelivery(ProductOrderPost req) {
            var order = _dbContext.ProductOrders.FirstOrDefault(o => o.Id == req.ProductOrderId && o.DealStatus == 3 && o.PostMode == "快递");
            if (order == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            order.DealStatus = 4;
            order.ShipCompany = req.PostCompany;
            order.ShipNum = req.PostNum;
            order.ShipTime = DateTime.UtcNow;
            _dbContext.ProductOrders.Update(order);
            return Result.Ok();
        }
        public Result UserConfigurePickupAddress(ProductOrderPost req) {
            var order = _dbContext.ProductOrders.FirstOrDefault(o => o.Id == req.ProductOrderId && o.DealStatus == 3 && o.PostMode == "自提");
            if (order == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            order.DealStatus = 4;
            order.ShipUsername = req.Username;
            order.ShipPhone = req.Phone;
            order.ShipAddress = req.Address;
            order.ShipTime = DateTime.UtcNow;
            _dbContext.ProductOrders.Update(order);
            return Result.Ok();
        }
        public Result UserConfirmDelivery(string orderId) {
            var order = _dbContext.ProductOrders.FirstOrDefault(o => o.Id == orderId && o.DealStatus == 4 && o.PostMode == "快递");
            if (order == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            order.DealStatus = 9;
            order.ShipTime = DateTime.UtcNow;
            _dbContext.ProductOrders.Update(order);
            return Result.Ok();
        }
        public Result UserFeedback(ProductOrderEvaluate req) {
            var user = _userService.GetCurrentUser();
            var order = _dbContext.ProductOrders.FirstOrDefault(o => o.Id == req.Id && o.UserId == user.Id && o.DealStatus == 9);
            if (order == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            order.EvaScore = req.Score;
            order.EvaContent = req.Content;
            order.DealStatus = 11;
            order.UpdateTime = DateTime.UtcNow;
            _dbContext.ProductOrders.Update(order);
            return Result.Ok();
        }
    }
}