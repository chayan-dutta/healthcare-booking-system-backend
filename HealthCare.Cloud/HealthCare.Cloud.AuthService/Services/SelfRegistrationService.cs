using HealthCare.Cloud.AuthService.Entities;
using HealthCare.Cloud.AuthService.Helpers;
using HealthCare.Cloud.AuthService.Models;
using HealthCare.Cloud.AuthService.Repository;
using HealthCare.Cloud.AuthService.ServiceClients;
using HealthCare.Common.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace HealthCare.Cloud.AuthService.Services;

/// <summary>
/// Handles the user self-registration process by orchestrating calls 
/// across different dependencies (Auth Repository, User Service, etc.).
/// 
/// Responsibilities:
/// - Validates whether a user already exists in the auth store.
/// - Creates a new user record in the external User Service.
/// - Creates corresponding authentication credentials in the Auth Repository.
/// - Builds and returns a strongly typed ApiResponse<UserRegistrationResponse>.
/// 
/// Notes:
/// - This class does not handle HTTP transport concerns (controllers do that).
/// - It encapsulates the "registration workflow" into a single cohesive service.
/// - Exceptions are caught and transformed into failure ApiResponse objects.
/// 
/// Dependencies:
/// - IConfiguration: Provides service base URLs and config values.
/// - IUserServiceClient: Client abstraction for calling the external User Service.
/// - IAuthRepository: Handles persistence of authentication credentials.
/// 
/// Example usage:
/// var response = await _registrationService.RegisterUserAsync(request);
/// 
/// To Do: Have to refactor as it is breaking Single Responsibility Principle from SOLID
/// </summary>
public partial class SelfRegistrationService : ISelfRegistrationService
{
    private readonly IConfiguration _configuration;
    private readonly IUserServiceClient _userServiceClient;
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<SelfRegistrationService> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="userServiceClient"></param>
    /// <param name="authRepository"></param>
    public SelfRegistrationService(IConfiguration configuration, 
        IUserServiceClient userServiceClient, 
        IAuthRepository authRepository,
        ILogger<SelfRegistrationService> logger)
    {
        _configuration = configuration;
        _userServiceClient = userServiceClient;
        _authRepository = authRepository;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ApiResponse<UserRegistrationResponse>> UserSelfRegisterAsync(UserRegistrationRequest request)
    {
        try
        {
            if (!ValidateRegistrationRequest(request))
                return Failure("Request Validation Failed", HttpStatusCode.BadRequest);

            if (await UserAlreadyExists(request.Email))
                return Failure("User already exists with given email", HttpStatusCode.Conflict);

            var userServiceResponse = await CreateUserInUserService(request);

            if (userServiceResponse == null)
                return Failure("User Service did not return proper data", HttpStatusCode.InternalServerError);

            var registrationResponse = await CreateAuthAndBuildResponse(request, userServiceResponse);

            return Success("Registration Successful. Validate email", registrationResponse, HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            SelfRegistrationServiceError(ex);
            return Failure(ex.Message, HttpStatusCode.InternalServerError);
        }
    }


    #region Private Helper Methods

    private static bool ValidateRegistrationRequest(UserRegistrationRequest userRegistrationRequest)
    {
        if (userRegistrationRequest == null)
            return false;

        // Email validation
        if (string.IsNullOrWhiteSpace(userRegistrationRequest.Email) ||
            !Regex.IsMatch(userRegistrationRequest.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return false;

        // Password validation (at least 8 chars, 1 upper, 1 lower, 1 number)
        if (string.IsNullOrWhiteSpace(userRegistrationRequest.Password) ||
            userRegistrationRequest.Password.Length < 8 ||
            !Regex.IsMatch(userRegistrationRequest.Password, @"[A-Z]") ||
            !Regex.IsMatch(userRegistrationRequest.Password, @"[a-z]") ||
            !Regex.IsMatch(userRegistrationRequest.Password, @"[0-9]"))
            return false;

        // First name & last name validation
        if (string.IsNullOrWhiteSpace(userRegistrationRequest.FullName))
            return false;

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private async Task<bool> UserAlreadyExists(string email) =>
        await _authRepository.IsAuthExistsAsync(email);

    /// <summary>
    /// Create the User 1st in the User Service
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<AddUserResponse?> CreateUserInUserService(UserRegistrationRequest request)
    {
        string userServiceBaseUrl = _configuration["UserServiceBaseUrl"] ??
            throw new InvalidOperationException("UserServiceBaseUrl not found in configuration");

        var response = await _userServiceClient.AddUser(request, userServiceBaseUrl);

        if (response?.IsSuccess == true
            && response.Status == HttpStatusCode.Created
            && response.Data != null)
        {
            return response.Data;
        }

        return null;
    }

    /// <summary>
    /// Add user data in Auth Service
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userDto"></param>
    /// <returns></returns>
    private async Task<UserRegistrationResponse> CreateAuthAndBuildResponse(UserRegistrationRequest request, AddUserResponse userResponse)
    {
        DateTime createdTime = await _authRepository.CreateAsync(
            PrepareAuthCredential(userResponse.UserId, request));

        return new UserRegistrationResponse
        {
            Email = request.Email,
            CreatedAt = createdTime,
            FullName = request.FullName
        };
    }

    /// <summary>
    /// Returns any success message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    private static ApiResponse<UserRegistrationResponse> Success(string message, UserRegistrationResponse data, HttpStatusCode status) =>
        new() { IsSuccess = true, Message = message, Data = data, Status = status };

    /// <summary>
    /// Returns any Failure response
    /// </summary>
    /// <param name="message"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    private static ApiResponse<UserRegistrationResponse> Failure(string message, HttpStatusCode status) =>
        new() { IsSuccess = false, Message = message, Data = null, Status = status };

    private static AuthCredential PrepareAuthCredential(Guid userId, UserRegistrationRequest registrationRequest)
    {
        (byte[] passHash, byte[] passSalt) = PasswordHelper.GeneratePasswordHash(registrationRequest.Password);

        return new AuthCredential()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Email = registrationRequest.Email,
            EmailVerificationExpiry = DateTime.UtcNow.AddHours(2),
            EmailVerificationToken = GenerateRandomString(),
            FailedLoginAttempts = 0,
            IsEmailVerified = false,
            IsFirstLogin = false,
            IsLocked = false,
            PasswordHash = Convert.ToBase64String(passHash),
            PasswordSalt = Convert.ToBase64String(passSalt),
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
        };
    }

    private static string GenerateRandomString()
    {
        // Generate a random unique validation code
        // Guid.NewGuid() generates a 128-bit globally unique identifier
        // ToString("N") converts the GUID into a 32-character hex string without hyphens
        //   - "D" -> default: 36 chars with hyphens (e.g., f47ac10b-58cc-4372-a567-0e02b2c3d479)
        //   - "N" -> 32 chars, no hyphens (e.g., f47ac10b58cc4372a5670e02b2c3d479)
        //   - "B" -> with braces (e.g., {f47ac10b-58cc-4372-a567-0e02b2c3d479})
        //   - "P" -> with parentheses (e.g., (f47ac10b-58cc-4372-a567-0e02b2c3d479))
        //   - "X" -> hex format (e.g., {0xf47ac10b,0x58cc,...})
        // Substring(0, 8) takes only the first 8 characters, giving a short code
        return Guid.NewGuid().ToString("N").Substring(0, 8);
    }

    #endregion


    #region Logger Messages

    [LoggerMessage(LogLevel.Error, Message = "{exception}")]
    partial void SelfRegistrationServiceError(Exception exception);

    #endregion
}
