using System.Text.Json.Serialization;

namespace CampusMarketApi.Core.Models.Responses;

public class RegisterResponse : BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
}

[JsonSerializable(typeof(RegisterResponse))]
internal partial class RegisterResponseJsonSerializerContext : JsonSerializerContext
{
}

public class RegisterResponseExample : ResponseExample<RegisterResponse>
{
    public override RegisterResponse GetExamples()
    {
        return new RegisterResponse
        {
            Success = true,
            Message = "When the registration is successful, this field will be null, otherwise it will contain an error message.",
            Token = "This is a JWT token."
        };
    }
}