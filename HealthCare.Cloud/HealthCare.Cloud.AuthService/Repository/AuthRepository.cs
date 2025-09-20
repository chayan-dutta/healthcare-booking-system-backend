using HealthCare.Cloud.AuthService.Data;
using HealthCare.Cloud.AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Cloud.AuthService.Repository;

/// <summary>
/// Repository class for DB related operations
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="dbContextFactory"></param>
public class AuthRepository(IDbContextFactory<AuthServiceDbContext> dbContextFactory) : IAuthRepository
{
    private readonly IDbContextFactory<AuthServiceDbContext> _dbContextFactory = dbContextFactory;

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
    /// Create an entry to the Auth table
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

   
    /// <inheritdoc />
    public async Task<AuthCredential> GetAuthDetailsByEmailAsync(string email)
    {
        using var authDbCtx = _dbContextFactory.CreateDbContext();
        return await authDbCtx.AuthCredentials.Where(a => a.Email == email).FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("No data found with the given email");
    }

    /// <summary>
    /// Update auth data 
    /// </summary>
    /// <param name="authCredential"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAuthAsync(AuthCredential authCredential)
    {
        await using var authDbCtx = _dbContextFactory.CreateDbContext(); // EF Core 6+ supports IAsyncDisposable
        authDbCtx.AuthCredentials.Update(authCredential);

        var affected = await authDbCtx.SaveChangesAsync();
        return affected > 0;
    }
}
