using HealthCare.Cloud.AuthService.Models;
using HealthCare.Cloud.AuthService.ServiceClients;
using HealthCare.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace HealthCare.Cloud.AuthService.Controllers;

/// <summary>
/// 
/// </summary>
[Tags("Public Auth APIs")]
[ApiController]
[Route("api/auth")]
public partial class PublicAuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ILogger<PublicAuthController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="userServiceClient"></param>
    /// <param name="logger"></param>
    public PublicAuthController(IConfiguration configuration
        , IUserServiceClient userServiceClient
        , ILogger<PublicAuthController> logger)
    {
        _configuration = configuration;
        _userServiceClient = userServiceClient;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("getstatus")]
    public string GetAuthAPIStatus() => "Running";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("selfregister")]
    public async Task<ApiResponse<UserRegistrationResponse>> UserSelfRegistration([FromBody] UserRegistrationRequest request)
    {
        if (!ValidateRegistrationRequest(request))
            return new ApiResponse<UserRegistrationResponse>
            {
                Data = null,
                IsSuccess = false,
                Message = "Request Validation Failed.",
                Status = System.Net.HttpStatusCode.BadRequest
            };

        string userServiceBaseUrl = _configuration["UserServiceBaseUrl"] ??
            throw new InvalidOperationException("UserServiceBaseUrl is not found in configuration");

        try
        {
            var responseFromUserService = await _userServiceClient.AddUser(request, userServiceBaseUrl);
            if (responseFromUserService.IsSuccess && responseFromUserService.Status == System.Net.HttpStatusCode.Created)
            {
                // TODO: Create logic for adding the user to the Auth DB
                // Testing branch protection
                Console.Write("User Added in User DB");
            }

            return new ApiResponse<UserRegistrationResponse>()
            {
                Data = new UserRegistrationResponse
                {
                    CreatedAt = DateTime.Now,
                    Email = request.Email,
                    FullName = request.FullName,
                    UserName = request.UserName
                }
            };
        }
        catch (Exception)
        {

            throw;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRegistrationRequest"></param>
    /// <returns></returns>
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
}
