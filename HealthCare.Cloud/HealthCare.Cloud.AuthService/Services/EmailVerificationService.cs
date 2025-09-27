using HealthCare.Cloud.AuthService.Entities;
using HealthCare.Cloud.AuthService.Models;
using HealthCare.Cloud.AuthService.Repository;
using HealthCare.Common.Models;
using System.Net;

namespace HealthCare.Cloud.AuthService.Services;

public partial class EmailVerificationService : IEmailVerificationService
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<EmailVerificationService> _logger;

    #region Constant messages used in responses

    private const string EmailTokenExpiredErrorMsg = "Email verification token expired.";
    private const string EmailIsAlreadyVerifiedMessage = "Your email is already verified";
    private const string NoAccountExistMessage = "No account exist associated with this email";
    private const string EmailVerificationSuccessMessage = "Email address successfully verified";
    private const string EmailVerificationTokenMismatchMsg = "Email verification token is not matching.";
    private const string EmailVerifictaionGeneralFailure = "Something went wrong. Please try later.";
    private const string EmptyToken = "Email Verification Token is required";

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new instance of <see cref="EmailVerificationService"/>.
    /// </summary>
    /// <param name="authRepository">Repository for authentication/authorization data.</param>
    /// <param name="logger">Logger for diagnostic messages.</param>
    public EmailVerificationService(
        IAuthRepository authRepository,
        ILogger<EmailVerificationService> logger)
    {
        _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    #region Interface Implementation

    /// <summary>
    /// Verifies a user's email address using the provided verification request.
    /// Applies a series of checks (token existence, expiry, already verified, token match)
    /// and updates the user's record if valid.
    /// </summary>
    /// <param name="request">The request containing the email and verification token.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating success or failure with details.</returns>
    public async Task<ApiResponse<VerifyEmailResponse>> VerifyEmailAsync(VerifyEmailRequest request)
    {
        try
        {
            // Validate request early to avoid null references
            if (request is null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrEmpty(request.EmailVerificationCode))
                return ErrorResponse(HttpStatusCode.BadRequest, request?.Email ?? string.Empty, EmptyToken);

            // Retrieve the auth credentials for this email
            var existingAuthEntry = await _authRepository.GetAuthDetailsByEmailAsync(request.Email);
            if (existingAuthEntry == null)
                return ErrorResponse(HttpStatusCode.NotFound, request.Email, NoAccountExistMessage);

            // Perform validation checks (expiry, already verified, token mismatch)
            var validationResult = ValidateEmailVerification(request, existingAuthEntry);
            if (validationResult != null)
                return validationResult;

            // Passed validation: update DB record to mark email verified
            var updateStatus = await UpdateEmailVerificationAsync(existingAuthEntry);
            if (!updateStatus)
                return ErrorResponse(HttpStatusCode.InternalServerError, request.Email, EmailVerifictaionGeneralFailure);

            // All good — return success
            return SuccessResponse(HttpStatusCode.OK, request.Email, EmailVerificationSuccessMessage);
        }
        catch (Exception ex)
        {
            // Log and return generic server error
            EmailVerificationServiceError(ex);
            return ErrorResponse(HttpStatusCode.InternalServerError, request.Email, ex.Message);
        }
    }

    #endregion

    #region Private helper methods

    /// <summary>
    /// Runs all pre-checks for verification (expiry, already verified, token mismatch)
    /// and returns an error response if validation fails.
    /// </summary>
    /// <param name="request">The incoming email verification request.</param>
    /// <param name="existingAuthEntry">The existing auth record from the database.</param>
    /// <returns>An error response if validation fails; otherwise <c>null</c> to continue.</returns>
    private static ApiResponse<VerifyEmailResponse>? ValidateEmailVerification(VerifyEmailRequest request, AuthCredential existingAuthEntry)
    {
        var now = DateTime.UtcNow;

        // Already verified
        if (existingAuthEntry.IsEmailVerified)
            return ErrorResponse(HttpStatusCode.BadRequest, request.Email, EmailIsAlreadyVerifiedMessage);

        // Token expired
        if (now > existingAuthEntry.EmailVerificationExpiry)
            return ErrorResponse(HttpStatusCode.Gone, request.Email, EmailTokenExpiredErrorMsg);

        // Token mismatch
        if (!string.Equals(existingAuthEntry.EmailVerificationToken, request.EmailVerificationCode, StringComparison.Ordinal))
            return ErrorResponse(HttpStatusCode.Unauthorized, request.Email, EmailVerificationTokenMismatchMsg);

        // All validations passed
        return null;
    }

    /// <summary>
    /// Updates the given auth credential to mark the email as verified and saves it to the database.
    /// </summary>
    /// <param name="existing">The existing auth credential to update.</param>
    /// <returns><c>true</c> if update succeeded; otherwise <c>false</c>.</returns>
    private async Task<bool> UpdateEmailVerificationAsync(AuthCredential existing)
    {
        existing.UpdatedAt = DateTime.UtcNow;
        existing.IsEmailVerified = true;
        return await _authRepository.UpdateAuthAsync(existing);
    }

    /// <summary>
    /// Builds an error <see cref="ApiResponse{T}"/> with a standardized format.
    /// </summary>
    private static ApiResponse<VerifyEmailResponse> ErrorResponse(HttpStatusCode status, string email, string message)
    {
        return BuildResponse(status, email, message, isError: true, verificationStatus: false);
    }

    /// <summary>
    /// Builds a success <see cref="ApiResponse{T}"/> with a standardized format.
    /// </summary>
    private static ApiResponse<VerifyEmailResponse> SuccessResponse(HttpStatusCode status, string email, string message)
    {
        return BuildResponse(status, email, message, isError: false, verificationStatus: true);
    }

    /// <summary>
    /// Creates a uniform <see cref="ApiResponse{T}"/> for both success and error scenarios.
    /// </summary>
    private static ApiResponse<VerifyEmailResponse> BuildResponse(HttpStatusCode status, string email, string message, bool isError, bool verificationStatus)
    {
        var emailVerifyResponse = new VerifyEmailResponse
        {
            Email = email,
            IsError = isError,
            VerificationStatus = verificationStatus,
            Message = message
        };

        return new ApiResponse<VerifyEmailResponse>
        {
            Data = emailVerifyResponse,
            Message = message,
            IsSuccess = !isError,
            Status = status
        };
    }

    #endregion

    #region Logger

    [LoggerMessage(LogLevel.Error, Message = "Exception caught at EmailVerificationService:")]
    partial void EmailVerificationServiceError(Exception exception);

    #endregion
}
