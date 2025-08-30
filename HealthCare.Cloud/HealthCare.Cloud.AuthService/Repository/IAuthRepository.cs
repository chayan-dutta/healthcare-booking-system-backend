using HealthCare.Cloud.AuthService.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace HealthCare.Cloud.AuthService.Repository;

public interface IAuthRepository
{
    /// <summary>
    /// Method to check if any user exists with the same email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> IsAuthExistsAsync(string email);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authCredential"></param>
    /// <returns>The Created Date and Time in UTC</returns>
    Task<DateTime> CreateAsync(AuthCredential authCredential);
}
