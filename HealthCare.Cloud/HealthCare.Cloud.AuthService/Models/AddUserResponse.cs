namespace HealthCare.Cloud.AuthService.Models;

/// <summary>
/// Response from the UserService
/// </summary>
public class AddUserResponse
{
    /// <summary>
    /// User Guid
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Email id of the user
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
