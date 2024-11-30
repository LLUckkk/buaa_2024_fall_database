using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using CampusMarketApi.Core.Models.Entities;
using Microsoft.Extensions.Configuration;
using CampusMarketApi.Core.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using CampusMarketApi.Infrastructure.Data;


namespace CampusMarketApi.Infrastructure.Services.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
            // TODO
            // Add additional claims as needed
        };

        var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string? ValidateToken(string token)
    {
        try
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured.");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is not configured.");
            var audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateLifetime = true
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        catch (Exception)
        {
            // Handle other exceptions
            // Log the exception or take appropriate action
            return null;
        }
    }
}