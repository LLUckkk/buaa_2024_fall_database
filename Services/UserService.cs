using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class UserService(ApplicationDbContext dbcontext, IPasswordHasher passwordHasher, ITokenService tokenService) : IUserService
    {
        private readonly ApplicationDbContext _dbcontext = dbcontext;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;


        public Result<string> Register(UserRegister req)
        {
            if (_dbcontext.Users.Any(u => u.Username == req.Username || u.Email == req.Email || u.StudentId == req.StudentId))
            {
                return Result<string>.Fail(ResultCode.Fail, "User already exists");
            }
            var user = CreateUser(req);
            return Result<string>.Ok(_tokenService.GenerateToken(user.Id));
        }

        public Result<string> Login(UserLogin req)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Username == req.Username);
            if (user == null || !_passwordHasher.VerifyPassword(req.Password, user.Password))
            {
                return Result<string>.Fail(ResultCode.NotFoundError, "Invalid username or password");
            }
            if (user.Status == 0)
            {
                return Result<string>.Fail(ResultCode.Fail, "User is disabled");
            }
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Result<string>.Ok(_tokenService.GenerateToken(user.Id));
        }

        public Result<UserInfo> GetUserInfo()
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            if (uid == null)
            {
                return Result<UserInfo>.Fail(ResultCode.NotFoundError);
            }
            var user = _dbcontext.Users.FirstOrDefault(u => u.Id == uid);
            if (user == null)
            {
                return Result<UserInfo>.Fail(ResultCode.NotFoundError);
            }
            return Result<UserInfo>.Ok(UserInfo.FromUser(user));
        }

        public Result<UserInfo> GetUserInfo(string id) {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Result<UserInfo>.Fail(ResultCode.NotFoundError);
            }
            return Result<UserInfo>.Ok(UserInfo.FromUser(user));
        }
        public Page<UserInfo> getUserList(UserAdminPage req) {
            var query = _dbcontext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(req.Key))
            {
                query = query.Where(u => u.Nickname.Contains(req.Key) || u.Email.Contains(req.Key) || u.Username.Contains(req.Key));
            }

            query = query.Where(u => u.CheckStatus == req.CheckStatus);
        

            query = query.OrderByDescending(u => u.CreateTime);

            var totalItems = query.Count();
            var users = query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();

            var page = new Page<UserInfo>
            {
                Items = users.Select(UserInfo.FromUser).ToList(),
                Total = totalItems,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            };

            return page;
        }
        public Result UpdateUserInfo(UpdateUserInfo req)
        {
            try
            {
                var uid = _tokenService.GetCurrentLoginUserId();
                if (uid == null)
                {
                    return Result.Fail(ResultCode.NotFoundError);
                }
                var user = _dbcontext.Users.FirstOrDefault(u => u.Id == uid);
                if (user == null)
                {
                    return Result.Fail(ResultCode.NotFoundError);
                }

                if (req.NickName != null)
                {
                    user.CheckNickName = req.NickName;
                }

                if (req.Intro != null)
                {
                    user.CheckIntro = req.Intro;
                }

                if (req.Avatar != null)
                {
                    user.CheckAvatar = req.Avatar;
                }

                user.CheckStatus = 0;

                _dbcontext.Users.Update(user);
                _dbcontext.SaveChanges();

                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(ResultCode.UpdateError);
            }
        }
        public Result UpdateUserPassword(UpdateUserInfo req)
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            if (uid == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            var user = _dbcontext.Users.FirstOrDefault(u => u.Id == uid);
            if (user == null)
            {
                return Result.Fail(ResultCode.NotFoundError);
            }
            if (req.Password == null || req.PasswordCheck == null)
            {
                return Result.Fail(ResultCode.ValidateError, "Password is required");
            }
            if (!req.Password.Equals(req.PasswordCheck))
            {
                return Result.Fail(ResultCode.ValidateError, "Password not match");
            }
            user.Password = _passwordHasher.HashPassword(req.Password);
            return Result.Ok();
        }

        private User CreateUser(UserRegister request)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Password = _passwordHasher.HashPassword(request.Password),
                Avatar = "default.png",
                Email = request.Email,
                Nickname = request.Username,
                StudentId = request.StudentId,
                Address = "Somewhere",
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Status = 9,
            };
            _dbcontext.Users.Add(user);
            _dbcontext.SaveChanges();
            return user;
        }

        public User? GetCurrentUser()
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            if (uid == null)
            {
                return null;
            }
            return _dbcontext.Users.FirstOrDefault(u => u.Id == uid);
        }

        public User? GetUserById(string id)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}