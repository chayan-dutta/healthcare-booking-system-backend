using HealthCare.Cloud.UserService.Models;

namespace HealthCare.Cloud.UserService.Repository;

/// <summary>
/// 
/// </summary>
public interface IUserDataRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<Guid> AddUserToDatabase(CreateUser user);
}
