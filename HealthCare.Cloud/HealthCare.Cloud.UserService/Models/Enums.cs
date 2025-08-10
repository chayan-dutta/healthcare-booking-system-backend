namespace HealthCare.Cloud.UserService.Models;

/// <summary>
/// Gender of the user
/// </summary>
public enum Gender
{
    Male = 0,
    Female = 1,
    Other = 2
}

/// <summary>
/// User role
/// </summary>
public enum UserRole
{
    Patient = 0,
    Doctor = 1,
    HospitalAdmin = 2,
    SuperAdmin = 3
}

public enum UserStatus
{
    /// <summary>
    /// Active when email is verified
    /// </summary>
    Active = 0,

    /// <summary>
    /// Email not verified, account locked due to fail attempts
    /// </summary>
    Inactive = 1,

    /// <summary>
    /// Suspended by admin
    /// </summary>
    Suspended = 2,

    /// <summary>
    /// Account deleted
    /// </summary>
    Deleted = 3
}