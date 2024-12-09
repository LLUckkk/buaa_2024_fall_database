using System.Linq;
using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class SystemUserService(ApplicationDbContext dbContext, IPasswordHasher passwordHasher, ITokenService tokenService) : ISystemUserService
    {
        private readonly ApplicationDbContext _dbcontext = dbContext;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;

        public Page<SystemRole> GetRolePage(SystemRolePage req)
        {
            var query = _dbcontext.SystemRoles.AsQueryable().Where(x => x.RoleName.Contains(req.Key) || x.RoleCode.Contains(req.Key)).OrderByDescending(x => x.CreateTime);
            return new Page<SystemRole>
            {
                Items = [.. query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize)],
                Total = query.Count(),
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            };
        }

        public Result CreateRole(SystemRoleCreate req)
        {
            if (string.IsNullOrEmpty(req.RoleCode) || string.IsNullOrEmpty(req.RoleName))
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            var role = new SystemRole
            {
                RoleCode = req.RoleCode,
                RoleName = req.RoleName,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            _dbcontext.SystemRoles.Add(role);
            var result = _dbcontext.SaveChanges();
            if (result == 0)
            {
                return Result.Fail(ResultCode.SaveError);
            }
            return Result.Ok();
        }

        public Result DeleteRole(string id)
        {
            var role = _dbcontext.SystemRoles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            var users = _dbcontext.SystemUsers.Where(u => u.RoleId == id).Count();
            if (users > 0)
            {
                return Result.Fail(ResultCode.DeleteError, "Role has users");
            }
            _dbcontext.SystemRoles.Remove(role);
            var result = _dbcontext.SaveChanges();
            if (result == 0)
            {
                return Result.Fail(ResultCode.DeleteError);
            }
            return Result.Ok();
        }

        public Result UpdateRole(SystemRoleUpdate req)
        {
            if (string.IsNullOrEmpty(req.Id))
            {
                return Result.Fail(ResultCode.ValidateError);
            }
            var role = _dbcontext.SystemRoles.FirstOrDefault(x => x.Id == req.Id);
            if (role == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            role.RoleCode = req.RoleCode;
            role.RoleName = req.RoleName;
            role.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dbcontext.SystemRoles.Update(role);
            var result = _dbcontext.SaveChanges();
            if (result == 0)
            {
                return Result.Fail(ResultCode.SaveError);
            }
            return Result.Ok();
        }

        public Result<List<SystemRole>> GetRoleList()
        {
            return Result<List<SystemRole>>.Ok(_dbcontext.SystemRoles.ToList());
        }

        public Result<string> Login(SystemUserLogin req)
        {
            SystemUser user = _dbcontext.SystemUsers.FirstOrDefault(x => x.Username == req.Username);
            if (user == null)
            {
                return Result<string>.Fail(ResultCode.NotFoundError);
            }
            if (!_passwordHasher.VerifyPassword(req.Password, user.Password))
            {
                return Result<string>.Fail(ResultCode.Fail);
            }
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Result<string>.Ok(_tokenService.GenerateToken(user.Id, user.RoleName));
        }

        public Result<SystemUserInfo> GetUserInfo()
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            if (uid == null)
            {
                return Result<SystemUserInfo>.Fail(ResultCode.NotFoundError);
            }
            var user = _dbcontext.SystemUsers.FirstOrDefault(u => u.Id == uid);
            if (user == null)
            {
                return Result<SystemUserInfo>.Fail(ResultCode.NotFoundError);
            }
            return Result<SystemUserInfo>.Ok(SystemUserInfo.FromSystemUser(user));
        }

        public Page<SystemUserInfo> GetUserList(SystemUserPage req)
        {
            var query = _dbcontext.SystemUsers.AsQueryable();
            if (!string.IsNullOrEmpty(req.Key))
            {
                query = query.Where(u => u.Username.Contains(req.Key) || u.Name.Contains(req.Key));
            }
            query = query.OrderByDescending(u => u.CreateTime);
            var totalItems = query.Count();
            var users = query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();
            var page = new Page<SystemUserInfo>
            {
                Items = users.Select(SystemUserInfo.FromSystemUser).ToList(),
                Total = totalItems,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            };
            return page;
        }
        public Result CreateSystemUser(SystemUserCreate req)
        {
            SystemRole role = _dbcontext.SystemRoles.FirstOrDefault(r => r.RoleCode == req.RoleCode);
            if (role == null)
            {
                return Result.Fail(ResultCode.ValidateError);
            }

            bool userExists = _dbcontext.SystemUsers.Any(u => u.Username == req.Username);
            if (userExists)
            {
                return Result.Fail(ResultCode.SaveError, "Username already exists");
            }

            SystemUser systemUser = new()
            {
                Username = req.Username,
                Password = _passwordHasher.HashPassword(req.Password),
                RoleId = role.Id,
                RoleCode = role.RoleCode,
                RoleName = role.RoleName,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Name = req.Name
            };

            _dbcontext.SystemUsers.Add(systemUser);
            _dbcontext.SaveChanges();

            return Result.Ok();
        }
        public Result UpdateSystemUser(SystemUserUpdate req)
        {
            if (string.IsNullOrEmpty(req.Id))
            {
                throw new ServiceException(ResultCode.ValidateError);
            }

            SystemUser systemUser = _dbcontext.SystemUsers.FirstOrDefault(u => u.Id == req.Id);
            if (systemUser == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }

            SystemRole role = _dbcontext.SystemRoles.FirstOrDefault(r => r.RoleCode == req.RoleCode);
            if (role == null)
            {
                return Result.Fail(ResultCode.ValidateError);
            }

            systemUser.Username = req.Username;
            systemUser.RoleId = role.Id;
            systemUser.RoleName = role.RoleName;
            systemUser.Name = req.Name;

            if (!string.IsNullOrEmpty(req.Password))
            {
                systemUser.Password = _passwordHasher.HashPassword(req.Password);
            }

            systemUser.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _dbcontext.SystemUsers.Update(systemUser);
            return Result.Ok();
        }

        public Result DeleteSystemUser(string id)
        {
            SystemUser systemUser = _dbcontext.SystemUsers.FirstOrDefault(u => u.Id == id);
            if (systemUser == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }

            _dbcontext.SystemUsers.Remove(systemUser);
            _dbcontext.SaveChanges();

            return Result.Ok();
        }
    }
}