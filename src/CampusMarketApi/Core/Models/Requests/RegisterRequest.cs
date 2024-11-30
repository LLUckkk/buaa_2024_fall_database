using System.Text.Json.Serialization;

namespace CampusMarketApi.Core.Models.Requests;

/// <summary>
/// Represents a register request with username, password, email, and student id.
/// All fields are required. Fields except for the password must be unique.
/// </summary>
public class RegisterRequest : BaseRequest
{
    /// <summary>
    ///   The username of the user. Must be unique.
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    ///  The password of the user, recommended to be hashed.
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    ///  The email of the user. Must be unique.
    ///  </summary>
    public string? Email { get; set; }
    /// <summary>
    /// The StudentId of the user. Must be unique.
    /// </summary>
    public string? StudentId { get; set; }
}


[JsonSerializable(typeof(RegisterRequest))]
internal partial class RegisterRequestJsonSerializerContext : JsonSerializerContext
{
}

public class RegisterRequestExample : RequestExample<RegisterRequest>
{
    public override RegisterRequest GetExamples()
    {
        return new RegisterRequest
        {
            Username = "yourcustomusername",
            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
            Email = "example@example.com",
            StudentId = "a1234567",
        };
    }
}