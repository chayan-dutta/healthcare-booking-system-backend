using HealthCare.Cloud.AuthService.Models;
using HealthCare.Common.Models;

namespace HealthCare.Cloud.AuthService.ServiceClients;

/// <summary>
/// 
/// </summary>
public interface IUserServiceClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRegistrationRequest"></param>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    Task<ApiResponse<object>> AddUser(UserRegistrationRequest userRegistrationRequest, string baseUrl);
}
