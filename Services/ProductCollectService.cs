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

        public Result Delete(string id)
        {
            var userid = _tokenService.GetCurrentLoginUserId();
            if (userid == null)
            {
                return Result.Fail(ResultCode.Fail);
            }
            ProductCollect collect = _dbContext.ProductCollects.FirstOrDefault(pc => pc.Id == id && pc.UserId == userid);
            if (collect == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            _dbContext.ProductCollects.Remove(collect);
            return Result.Ok();
        }
    }
}