using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class UserAddressService(ApplicationDbContext dbContext, ITokenService tokenService) : IUserAddressService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ITokenService _tokenService = tokenService;

        public Result AddUserAddress(UserAddressObj req)
        {
            var userid = _tokenService.GetCurrentLoginUserId();
            var userAddress = new UserAddress
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userid,
                Address = req.Address,
                Phone = req.Phone,
                Name = req.Name,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            _dbContext.UserAddresses.Add(userAddress);
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        public Result DeleteUserAddress(string id)
        {
            var userid = _tokenService.GetCurrentLoginUserId();
            var userAddress = _dbContext.UserAddresses.FirstOrDefault(ua => ua.Id == id);
            if (userAddress == null || userAddress.UserId != userid)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }

            _dbContext.UserAddresses.Remove(userAddress);
            _dbContext.SaveChanges();

            return Result.Ok();
        }

        public Result<List<UserAddress>> GetUserAddressList()
        {
            var userId = _tokenService.GetCurrentLoginUserId();
            return Result<List<UserAddress>>.Ok(_dbContext.UserAddresses.Where(ua => userId != null && ua.UserId == userId).ToList());
        }
    }
}