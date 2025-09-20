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
    /// Method to create a auth entry
    /// </summary>
    /// <param name="authCredential"></param>
    /// <returns>The Created Date and Time in UTC</returns>
    Task<DateTime> CreateAsync(AuthCredential authCredential);

    /// <summary>
    /// Get auth data using an email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<AuthCredential> GetAuthDetailsByEmailAsync(string email);

    /// <summary>
    /// Update auth entry
    /// </summary>
    /// <param name="authCredential"></param>
    /// <returns></returns>
    Task<bool> UpdateAuthAsync(AuthCredential authCredential);
}
