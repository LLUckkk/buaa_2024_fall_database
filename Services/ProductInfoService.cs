using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductInfoService(ApplicationDbContext dbContext, IUserService userService) : IProductInfoService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public Result CreateProductInfo(ProductInfoObj req)
        {
            var user = _userService.GetCurrentUser();
            var pi = new ProductInfo
            {
                Id = Guid.NewGuid().ToString(),
                Title = req.Title,
                Intro = req.Intro,
                Image = req.Image,
                Adcode = req.Adcode,
                Province = req.Province,
                City = req.City,
                District = req.District,
                Price = (long)req.Price * 100,
                OriginalPrice = (long)req.OriginalPrice * 100,
                Status = 1,
                LikeCount = 0,
                UserId = user!.Id,
                CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
            };
            if (!string.IsNullOrEmpty(req.Type))
            {
                var type = _dbContext.ProductTypes.FirstOrDefault(t => t.TypeCode == req.Type);
                if (type != null)
                {
                    pi.TypeCode = type.TypeCode;
                    pi.TypeName = type.TypeName;
                }
            }
            _dbContext.ProductInfos.Add(pi);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }
        public Result<List<ProductInfoDetail>> GetProductInfoList(ProductInfoPage req)
        {
            var user = _userService.GetCurrentUser();
            var query = _dbContext.ProductInfos.AsQueryable();

            if (!string.IsNullOrEmpty(req.TypeCode))
            {
                query = query.Where(pi => pi.TypeCode == req.TypeCode);
            }

            query = query.Where(pi => pi.Status == 9);

            if (!string.IsNullOrEmpty(req.Key))
            {
                query = query.Where(pi => pi.Title.Contains(req.Key));
            }

            query = query.OrderByDescending(pi => pi.CreateTime);

            var page = query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();

            var list = page.Select(item => new ProductInfoDetail
            {
                Id = item.Id,
                Title = item.Title,
                Intro = item.Intro,
                Image = item.Image,
                Adcode = item.Adcode,
                Province = item.Province,
                City = item.City,
                District = item.District,
                Price = item.Price,
                OriginalPrice = item.OriginalPrice,
                Status = item.Status,
                LikeCount = item.LikeCount,
                UserId = item.UserId,
                CreateTime = item.CreateTime,
                UpdateTime = item.UpdateTime,
                Avatar = user?.Avatar
            }
            ).ToList();

            return Result<List<ProductInfoDetail>>.Ok(list);
        }
        public Result<ProductInfoDetail> GetProductInfo(string id)
        {
            var info = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (info == null)
            {
                return Result<ProductInfoDetail>.Fail(ResultCode.NotFoundError);
            }
            var user = _userService.GetUserById(info.UserId);
            if (user == null)
            {
                return Result<ProductInfoDetail>.Fail(ResultCode.NotFoundError);
            }
            var detail = new ProductInfoDetail
            {
                Id = info.Id,
                Title = info.Title,
                Intro = info.Intro,
                Image = info.Image,
                Price = info.Price,
                OriginalPrice = info.OriginalPrice,
                PostType = info.PostType,
                Adcode = info.Adcode,
                Province = info.Province,
                City = info.City,
                District = info.District,
                Status = info.Status,
                UserId = info.UserId,
                CreateTime = info.CreateTime,
                UpdateTime = info.UpdateTime,
                UserInfo = user,
            };
            return Result<ProductInfoDetail>.Ok(detail);
        }

        public Result<List<ProductInfo>> GetMyProductInfoList()
        {
            var user = _userService.GetCurrentUser();

            var list = _dbContext.ProductInfos
                .Where(pi => pi.UserId == user!.Id && pi.Status != 10)
                .OrderByDescending(pi => pi.CreateTime)
                .ToList();

            return Result<List<ProductInfo>>.Ok(list);
        }
        public Result CreateLike(string id)
        {
            var info = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (info == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            info.LikeCount += 1;
            _dbContext.ProductInfos.Update(info);
            return Result.Ok();
        }
        public Result<Page<ProductInfoDetail>> GetProductInfoPage(SystemProductInfoPage req)
        {
            var query = _dbContext.ProductInfos.AsQueryable();

            if (!string.IsNullOrEmpty(req.Status))
            {
                query = query.Where(pi => pi.Status.ToString() == req.Status);
            }

            if (!string.IsNullOrEmpty(req.Key))
            {
                query = query.Where(pi => pi.Title.Contains(req.Key) || pi.TypeName!.Contains(req.Key));
            }

            query = query.Where(pi => pi.Status != 10)
                         .OrderByDescending(pi => pi.CreateTime);

            var page = query.Skip((req.PageNumber - 1) * req.PageSize)
                            .Take(req.PageSize)
                            .Select(pi => new ProductInfoDetail
                            {
                                Id = pi.Id,
                                Title = pi.Title,
                                Intro = pi.Intro,
                                Image = pi.Image,
                                Price = pi.Price,
                                OriginalPrice = pi.OriginalPrice,
                                PostType = pi.PostType,
                                Adcode = pi.Adcode,
                                Province = pi.Province,
                                City = pi.City,
                                District = pi.District,
                                Status = pi.Status,
                                UserId = pi.UserId,
                                CreateTime = pi.CreateTime,
                                UpdateTime = pi.UpdateTime,
                                UserInfo = _userService.GetUserById(pi.UserId)
                            })
                            .ToList();

            return Result<Page<ProductInfoDetail>>.Ok(new Page<ProductInfoDetail>
            {
                Items = page,
                Total = query.Count(),
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            });
        }
        public Result<ProductInfoAdminDetail> GetProductInfoAdminDetail(string id)
        {
            var info = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (info == null)
            {
                return Result<ProductInfoAdminDetail>.Fail(ResultCode.NotFoundError);
            }
            var user = _userService.GetUserById(info.UserId);
            if (user == null)
            {
                return Result<ProductInfoAdminDetail>.Fail(ResultCode.NotFoundError);
            }
            var detail = new ProductInfoAdminDetail
            {
                User = user,
                ProductInfo = info
            };
            return Result<ProductInfoAdminDetail>.Ok(detail);
        }
        public Result<List<ProductInfoDetail>> GetMyProductCollectionInfoList()
        {
            var user = _userService.GetCurrentUser();

            var collectList = _dbContext.ProductCollects.Where(pc => pc.UserId == user!.Id).ToList();
            var collectInfoList = collectList.Select(collect =>
            {
                var productInfo = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == collect.ProductId);
                if (productInfo == null)
                {
                    return null;
                }
                var detail = new ProductInfoDetail
                {
                    Id = productInfo.Id,
                    Title = productInfo.Title,
                    Intro = productInfo.Intro,
                    Image = productInfo.Image,
                    Price = productInfo.Price,
                    OriginalPrice = productInfo.OriginalPrice,
                    PostType = productInfo.PostType,
                    Adcode = productInfo.Adcode,
                    Province = productInfo.Province,
                    City = productInfo.City,
                    District = productInfo.District,
                    Status = productInfo.Status,
                    UserId = productInfo.UserId,
                    CreateTime = productInfo.CreateTime,
                    UpdateTime = productInfo.UpdateTime,
                    UserInfo = _userService.GetUserById(productInfo.UserId)
                };
                var productVoucher = _dbContext.ProductVouchers.FirstOrDefault(pv => pv.ProductId == productInfo.Id);
                if (productVoucher != null)
                {
                    detail.ProductVoucher = productVoucher;
                }
                return detail;
            }).Where(pi => pi != null).ToList();

            return Result<List<ProductInfoDetail>>.Ok(collectInfoList!);
        }
        public Result ApproveProduct(string id)
        {
            var productInfo = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (productInfo == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            if (productInfo.Status != 1)
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            productInfo.Status = 9;
            _dbContext.ProductInfos.Update(productInfo);
            _dbContext.SaveChanges();
            return Result.Ok();
        }
        public Result RejectProduct(string id)
        {
            var productInfo = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (productInfo == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            if (productInfo.Status != 1)
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            productInfo.Status = 2;
            _dbContext.ProductInfos.Update(productInfo);
            _dbContext.SaveChanges();
            return Result.Ok();
        }
        public Result HideProduct(string id)
        {
            var productInfo = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (productInfo == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            if (productInfo.Status != 9)
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            productInfo.Status = 2;
            _dbContext.ProductInfos.Update(productInfo);
            _dbContext.SaveChanges();
            return Result.Ok();
        }
        public Result<long> GetTodayCount()
        {
            var startDay = new DateTimeOffset(DateTime.UtcNow.Date).ToUnixTimeSeconds();
            var endDay = new DateTimeOffset(DateTime.UtcNow.Date.AddDays(1)).ToUnixTimeSeconds();
            var count = _dbContext.ProductInfos.Count(pi => pi.CreateTime >= startDay && pi.CreateTime < endDay);
            return Result<long>.Ok(count);
        }
        public Result<long> GetMonthCount()
        {
            var startDay = new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)).ToUnixTimeSeconds();
            var endDay = new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month + 1, 1)).ToUnixTimeSeconds();
            var count = _dbContext.ProductInfos.Count(pi => pi.CreateTime >= startDay && pi.CreateTime < endDay);
            return Result<long>.Ok(count);
        }

        public Result RemoveById(string id) {
            var productInfo = _dbContext.ProductInfos.FirstOrDefault(pi => pi.Id == id);
            if (productInfo == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            _dbContext.ProductInfos.Remove(productInfo);
            return Result.Ok();
        }
    }
}