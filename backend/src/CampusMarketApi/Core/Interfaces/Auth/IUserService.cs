using CampusMarketApi.Core.Models.Entities;
using CampusMarketApi.Core.Models.Requests;
using CampusMarketApi.Core.Models.Responses;

namespace CampusMarketApi.Core.Interfaces.Auth;

public interface IUserService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);

    User? ValidateToken(string token);
}