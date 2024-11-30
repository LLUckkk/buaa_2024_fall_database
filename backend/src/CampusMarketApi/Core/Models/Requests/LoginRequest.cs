using System.Text.Json.Serialization;

namespace CampusMarketApi.Core.Models.Requests;

/// <summary>
/// Represents a login request with username and password.
/// </summary>
public class LoginRequest : BaseRequest
{
    /// <summary>
    /// The username for the login request.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// The password for the login request.
    /// </summary>
    public string? Password { get; set; }
}

[JsonSerializable(typeof(LoginRequest))]
internal partial class LoginRequestJsonSerializerContext : JsonSerializerContext
{

}

public class LoginRequestExample : RequestExample<LoginRequest>
{
    public override LoginRequest GetExamples()
    {
        return new LoginRequest
        {
            Username = "yourcustomusername",
            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b"
        };
    }
}