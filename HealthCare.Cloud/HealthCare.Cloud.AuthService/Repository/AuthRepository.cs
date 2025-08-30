using HealthCare.Cloud.AuthService.Data;
using HealthCare.Cloud.AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Cloud.AuthService.Repository;

/// <summary>
/// Repository class for DB related operations
/// </summary>
public class AuthRepository : IAuthRepository
{
    private readonly IDbContextFactory<AuthServiceDbContext> _dbContextFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContextFactory"></param>
    public AuthRepository(IDbContextFactory<AuthServiceDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    /// <summary>
    /// Method to check if any user exists with the same email while user registration
    /// </summary>
    /// <param name="email">email id of the user</param>
    /// <returns>boolean value</returns>
    public async Task<bool> IsAuthExistsAsync(string email)
    {
        using var authDbContext = _dbContextFactory.CreateDbContext();

        return await authDbContext.AuthCredentials
            .AnyAsync(a => a.Email == email);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="authCredential"></param>
    /// <returns></returns>
    public async Task<DateTime> CreateAsync(AuthCredential authCredential)
    {
        using var userAuthDbContext = _dbContextFactory.CreateDbContext();

        userAuthDbContext.AuthCredentials.Add(authCredential);
        await userAuthDbContext.SaveChangesAsync();

        return authCredential.CreatedAt;
    }


}
