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
            if (req.Address == null || req.Phone == null || req.Name == null)
                return Result.Fail(ResultCode.ValidateError);
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

        public Result UpdateUserAddress(UserAddressObj req)
        {
            var userid = _tokenService.GetCurrentLoginUserId();
            var userAddress = _dbContext.UserAddresses.FirstOrDefault(ua => ua.Id == req.Id);
            if (userAddress == null || userAddress.UserId != userid)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }

            if (req.Name != null)
                userAddress.Name = req.Name;
            if (req.Phone != null)
                userAddress.Phone = req.Phone;
            if (req.Address != null)
                userAddress.Address = req.Address;
            if (req.Name != null || req.Phone != null || req.Address != null)
            {
                userAddress.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _dbContext.UserAddresses.Update(userAddress);
                var save = _dbContext.SaveChanges();
                if (save == 0)
                    return Result.Fail(ResultCode.SaveError);
            }
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
            var save = _dbContext.SaveChanges();
            if (save == 0)
                return Result.Fail(ResultCode.SaveError);
            return Result.Ok();
        }

        public Result<List<UserAddressObj>> GetUserAddressList()
        {
            var userId = _tokenService.GetCurrentLoginUserId();
            return Result<List<UserAddressObj>>.Ok(_dbContext.UserAddresses.Where(ua => userId != null && ua.UserId == userId).ToList().OrderByDescending(ua => ua.CreateTime).Select(ua => new UserAddressObj
            {
                Id = ua.Id,
                Name = ua.Name,
                Phone = ua.Phone,
                Address = ua.Address,
            }).ToList());
        }
    }
}