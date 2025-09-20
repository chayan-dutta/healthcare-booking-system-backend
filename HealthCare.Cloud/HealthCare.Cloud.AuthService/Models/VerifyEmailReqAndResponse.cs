namespace HealthCare.Cloud.AuthService.Models;

/// <summary> Verify email base class </summary>
public class VerifyEmailBase
{
    /// <summary> email address </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary> Verify email request body </summary>
public class VerifyEmailRequest : VerifyEmailBase
{
    /// <summary> email verification code </summary>
    public string EmailVerificationCode { get; set; } = string.Empty;
}

/// <summary> Verify email API Response </summary>
public class VerifyEmailResponse : VerifyEmailBase
{
    /// <summary> verification status </summary>
    public bool VerificationStatus { get; set; } = false;

    /// <summary> is any error occurred </summary>
    public bool IsError { get; set; } = false;

    /// <summary> errormessage </summary>
    public string Message { get; set; } = string.Empty;
}
