using HealthCare.Cloud.AuthService.Models;
using HealthCare.Common.Models;

namespace HealthCare.Cloud.AuthService.Services;

public interface IEmailVerificationService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResponse<VerifyEmailResponse>> VerifyEmailAsync(VerifyEmailRequest request);
}
