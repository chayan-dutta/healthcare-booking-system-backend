namespace HealthCare.Common.Models.User;

/// <summary>
/// Response model for Adding an user
/// </summary>
public class AddUserResponse
{
    /// <summary>
    /// User Id
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Email id of User
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
