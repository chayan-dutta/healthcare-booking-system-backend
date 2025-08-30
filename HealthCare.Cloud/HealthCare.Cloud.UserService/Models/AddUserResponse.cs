namespace HealthCare.Cloud.UserService.Models;

/// <summary>
/// Response for adding user
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
