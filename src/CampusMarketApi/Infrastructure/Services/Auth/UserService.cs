using CampusMarketApi.Core.Models.Requests;
using CampusMarketApi.Core.Models.Responses;
using CampusMarketApi.Core.Interfaces.Auth;
using CampusMarketApi.Infrastructure.Data;
using CampusMarketApi.Core.Models.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CampusMarketApi.Infrastructure.Services.Auth;

public class UserService : IUserService {
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public UserService(ApplicationDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (request.Username == null || request.Password == null)
        {
            return Task.FromResult(new LoginResponse
            {
                Success = false,
                Message = "Missing required fields",
            });
        }
        var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            return Task.FromResult(new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password",
            });
        }
        user.LastLoginAt = DateTime.UtcNow;
        var token = _tokenService.GenerateToken(user);
        return Task.FromResult(new LoginResponse
        {
            Success = true,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(12),
        });
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (request.Username == null || request.Password == null || request.Email == null || request.StudentId == null)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Missing required fields",
            };
        }
        if (request.Username.Length < 4 || request.Username.Length > 64)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Username must be between 4 and 64 characters",
            };
        }
        if (!new EmailAddressAttribute().IsValid(request.Email))
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Invalid email address",
            };
        }
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Username already exists",
            };
        }
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Email already exists",
            };
        }
        if (await _context.Users.AnyAsync(u => u.StudentId == request.StudentId))
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Student ID already exists",
            };
        }
        var user = await CreateUser(request);
        var token = _tokenService.GenerateToken(user);
        return new RegisterResponse
        {
            Success = true,
            Token = token,
        };
    }

    public User? ValidateToken(string token)
    {
        var username = _tokenService.ValidateToken(token);
        if (username == null)
        {
            return null;
        }
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    private async Task<User> CreateUser(RegisterRequest request)
    {
        var user = new User
        {
            Username = request.Username!,
            Password = _passwordHasher.HashPassword(request.Password!),
            Email = request.Email!,
            Nickname = request.Username!,
            IsAdmin = false,
            IsVerified = false,
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}