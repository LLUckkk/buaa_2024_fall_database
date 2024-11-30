using System.Text.Json.Serialization;

namespace CampusMarketApi.Core.Models.Responses;

public class LoginResponse : BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
}

[JsonSerializable(typeof(LoginResponse))]
internal partial class AuthResponseJsonSerializerContext : JsonSerializerContext
{
}

public class LoginResponseExample : ResponseExample<LoginResponse>
{
    public override LoginResponse GetExamples()
    {
        return new LoginResponse
        {
            Success = true,
            Message = "When the login is successful, this field will be null, otherwise it will contain an error message.",
            Token = "This is a JWT token. Save this token in the Authorization header for future requests.",
            Expiration = DateTime.UtcNow.AddHours(1)
        };
    }
}