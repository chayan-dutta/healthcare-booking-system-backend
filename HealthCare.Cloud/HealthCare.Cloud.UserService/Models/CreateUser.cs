using System.Text.Json.Serialization;

namespace HealthCare.Cloud.UserService.Models;

/// <summary>
/// Create user model
/// </summary>
public class CreateUser
{
    /// <summary>
    /// email id
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    [JsonPropertyName("fullname")]
    public required string FullName { get; set; }

    /// <summary>
    /// Role
    /// </summary>
    [JsonPropertyName("role")]
    public UserRole Role { get; set; } = UserRole.Patient;

    /// <summary>
    /// Hospital Id
    /// </summary>
    [JsonPropertyName("hospitalId")]
    public Guid HospitalId { get; set; } = Guid.Empty;

    /// <summary>
    /// Phone Number
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gender
    /// </summary>
    [JsonPropertyName("gender")]
    public Gender Gender { get; set; } = Gender.Other;

    /// <summary>
    /// Date of birth
    /// </summary>
    [JsonPropertyName("dob")]
    public DateTime DOB { get; set; }

    /// <summary>
    /// address
    /// </summary>
    [JsonPropertyName("address")]
    public Address? Address { get; set; }
}
