using HealthCare.Cloud.AuthService.Models;
using HealthCare.Common.Models;

namespace HealthCare.Cloud.AuthService.Services;

public interface ISelfRegistrationService
{
    /// <summary>
    /// Service of user self registration
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResponse<UserRegistrationResponse>> UserSelfRegisterAsync(UserRegistrationRequest request);
}
