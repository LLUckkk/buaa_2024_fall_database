using Market.Constants;
using Market.Entities;
using Market.Interfaces;
using Market.Models;

namespace Market.Services
{
    public class UserService(ApplicationDbContext dbcontext, IPasswordHasher passwordHasher, ITokenService tokenService) : IUserService
    {
        private readonly ApplicationDbContext _dbContext = dbcontext;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;


        public Result<string> Register(UserRegister req)
        {
            if (_dbContext.Users.Any(u => u.Username == req.Username || u.Email == req.Email || u.StudentId == req.StudentId))
            {
                return Result<string>.Fail(ResultCode.Fail, "User already exists");
            }
            var user = CreateUser(req);
            return Result<string>.Ok(_tokenService.GenerateToken(user.Id, "User"));
        }

        public Result<string> Login(UserLogin req)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == req.Username);
            if (user == null || !_passwordHasher.VerifyPassword(req.Password, user.Password))
            {
                return Result<string>.Fail(ResultCode.NotFoundError, "Invalid username or password");
            }
            if (user.Status == 0)
            {
                return Result<string>.Fail(ResultCode.Fail, "User is disabled");
            }
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Result<string>.Ok(_tokenService.GenerateToken(user.Id, "User"));
        }

        public Result<string> GetValidateToken(string email) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return Result<string>.Fail(ResultCode.NotFoundError, "User not found");
            }
            return Result<string>.Ok("123456");
        }
        public Result ResetPassword(ResetPasswordObj req) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == req.Email);
            if (user == null)
            {
                return Result.Fail(ResultCode.NotFoundError, "User not found");
            }
            if (string.IsNullOrEmpty(req.NewPassword)) {
                return Result.Fail(ResultCode.ValidateError, "Password is required");
            }
            if(req.Token != "123456") {
                return Result.Fail(ResultCode.ValidateError, "Invalid token");
            }
            user.Password = _passwordHasher.HashPassword(req.NewPassword);
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return Result.Ok();
        }

        public Result<User> GetUser()
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == uid);
            if (user == null)
            {
                return Result<User>.Fail(ResultCode.NotFoundError);
            }
            return Result<User>.Ok(user);
        }

        public Result<User> GetUser(string id) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Result<User>.Fail(ResultCode.NotFoundError);
            }
            return Result<User>.Ok(user);
        }
        public Result<Page<User>> GetUserList(UserAdminPage req) {
            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(req.Key))
            {
                query = query.Where(u => u.Nickname.Contains(req.Key) || u.Email.Contains(req.Key) || u.Username.Contains(req.Key));
            }

            query = query.Where(u => u.CheckStatus == req.CheckStatus);
        

            query = query.OrderByDescending(u => u.CreateTime);

            var totalItems = query.Count();
            var users = query.Skip((req.PageNumber - 1) * req.PageSize).Take(req.PageSize).ToList();

            var page = new Page<User>
            {
                Items = users.ToList(),
                Total = totalItems,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize
            };

            return Result<Page<User>>.Ok(page);
        }
        public Result UpdateUserInfo(UpdateUserInfo req)
        {
            try
            {
                var uid = _tokenService.GetCurrentLoginUserId();
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == uid);
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

                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();

                return Result.Ok();
            }
            catch (Exception)
            {
                return Result.Fail(ResultCode.UpdateError);
            }
        }
        public Result UpdateUserPassword(UpdateUserInfo req)
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == uid);
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
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return Result.Ok();
        }

        private User CreateUser(UserRegister request)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username!,
                Password = _passwordHasher.HashPassword(request.Password!),
                Avatar = "http://localhost:8080/image/default.png",
                Email = request.Email!,
                Nickname = request.Username!,
                StudentId = request.StudentId!,
                Address = "Somewhere",
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Status = 9,
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User? GetCurrentUser()
        {
            var uid = _tokenService.GetCurrentLoginUserId();
            return _dbContext.Users.FirstOrDefault(u => u.Id == uid);
        }

        public User? GetUserById(string id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public Result EnableUser(string id) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Result.Fail(ResultCode.NotFoundError, "User not found");
            }
            user.Status = 9;
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return Result.Ok();
        }
        public Result DisableUser(string id) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Result.Fail(ResultCode.NotFoundError, "User not found");
            }
            user.Status = 0;
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return Result.Ok();
        }
        public Result ApproveUserProfileUpdate(string id) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Result.Fail(ResultCode.NotFoundError, "User not found");
            }

            if (!string.IsNullOrEmpty(user.CheckAvatar))
            {
                user.Avatar = user.CheckAvatar;
            }
            if (!string.IsNullOrEmpty(user.CheckNickName))
            {
                user.Nickname = user.CheckNickName;
            }
            if (!string.IsNullOrEmpty(user.CheckIntro))
            {
                user.Intro = user.CheckIntro;
            }

            user.CheckAvatar = string.Empty;
            user.CheckNickName = string.Empty;
            user.CheckIntro = string.Empty;
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            user.CheckStatus = 9;

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            return Result.Ok();
        }
        public Result RejectUserProfileUpdate(string id) {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Result.Fail(ResultCode.NotFoundError, "User not found");
            }

            user.CheckStatus = 7;
            user.UpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            return Result.Ok();
        }
    }
}