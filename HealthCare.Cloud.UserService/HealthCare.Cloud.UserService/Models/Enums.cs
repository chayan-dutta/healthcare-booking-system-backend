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
    Active = 0,
    Inactive = 1,
    Suspended = 2,
    Deleted = 3
}