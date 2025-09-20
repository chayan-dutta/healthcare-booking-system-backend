using HealthCare.Cloud.AuthService.Models;
using HealthCare.Cloud.AuthService.Services;
using HealthCare.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Cloud.AuthService.Controllers;

/// <summary>
/// 
/// </summary>
[Tags("Public Auth APIs")]
[ApiController]
[Route("api/auth")]
public partial class PublicAuthController : ControllerBase
{
    private readonly ISelfRegistrationService _selfRegistrationService;
    private readonly IEmailVerificationService _emailVerificationService;

    public PublicAuthController(
        ISelfRegistrationService selfRegistrationService,
        IEmailVerificationService emailVerificationService)
    {
        _selfRegistrationService = selfRegistrationService;
        _emailVerificationService = emailVerificationService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("getstatus")]
    public string GetAuthAPIStatus() => "Running";

    /// <summary>
    /// Self registration
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("selfregister")]
    public async Task<ApiResponse<UserRegistrationResponse>> UserSelfRegistration([FromBody] UserRegistrationRequest request)
    {
        return await _selfRegistrationService.UserSelfRegisterAsync(request);
    }

    /// <summary>
    /// Verify email address
    /// </summary>
    /// <param name="verifyEmailRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("verifyemail")]
    public async Task<ApiResponse<VerifyEmailResponse>> VerifyEmailAddress([FromBody] VerifyEmailRequest verifyEmailRequest)
    {
        return await _emailVerificationService.VerifyEmailAsync(verifyEmailRequest);
    }

}