using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class ProductCollectService(ApplicationDbContext dbContext, ITokenService tokenService) : IProductCollectService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ITokenService _tokenService = tokenService;

        public Result Create(string req)
        {
            var userId = _tokenService.GetCurrentLoginUserId();
            var existingCollect = _dbContext.ProductCollects
                .FirstOrDefault(pc => pc.UserId == userId && pc.ProductId == req);

            if (existingCollect != null)
            {
                return Result.Ok();
            }

            var productCollect = new ProductCollect
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = req,
                CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
            };

            _dbContext.ProductCollects.Add(productCollect);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        public Result Delete(string id)
        {
            var userId = _tokenService.GetCurrentLoginUserId();
            var collect = _dbContext.ProductCollects.FirstOrDefault(pc => pc.Id == id && pc.UserId == userId);
            if (collect == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            _dbContext.ProductCollects.Remove(collect);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        public Result<List<string>> GetCollectionProductIdList()
        {
            var userId = _tokenService.GetCurrentLoginUserId();

            var productCollects = _dbContext.ProductCollects
                .Where(pc => pc.UserId == userId)
                .ToList()
                .Select(pc => pc.ProductId);

            return Result<List<string>>.Ok(productCollects.ToList());
        }
    }
}