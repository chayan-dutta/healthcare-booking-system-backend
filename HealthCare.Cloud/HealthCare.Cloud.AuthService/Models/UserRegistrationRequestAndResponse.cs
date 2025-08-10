using HealthCare.Cloud.AuthSevice.Models;
using System.Text.Json.Serialization;

namespace HealthCare.Cloud.AuthService.Models;


public class UserRegistrationBase
{
    /// <summary>
    /// username
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;
}

/// <summary>
/// Request
/// </summary>
public class UserRegistrationRequest : UserRegistrationBase
{
    /// <summary>
    /// password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;   

    /// <summary>
    /// Gender
    /// </summary>
    [JsonPropertyName("gender")]
    public GenderType Gender { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    [JsonPropertyName("dob")]
    public DateOnly Dob { get; set; }
}

/// <summary>
/// Response
/// </summary>
public class UserRegistrationResponse : UserRegistrationBase
{
    /// <summary>
    /// User created at
    /// </summary>
    [JsonPropertyName("createdat")]
    public DateTime CreatedAt { get; set; }
}
