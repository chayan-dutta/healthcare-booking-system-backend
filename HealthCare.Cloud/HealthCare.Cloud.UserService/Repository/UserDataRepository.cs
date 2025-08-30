using HealthCare.Cloud.UserService.Data;
using HealthCare.Cloud.UserService.Models;

namespace HealthCare.Cloud.UserService.Repository;

/// <summary>
/// 
/// </summary>
/// <param name="userDbContext"></param>
/// <param name="logger"></param>
public partial class UserDataRepository(UserDbContext userDbContext, ILogger<UserDataRepository> logger) : IUserDataRepository
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
    public async Task<Guid> AddUserToDatabase(CreateUser user)
    {
        Guid hospitalId = user.Role == UserRole.Patient ? Guid.Empty : user.HospitalId;

        var entity = new User
        {
            Id = Guid.NewGuid(),
            FullName = user.FullName,
            Email = user.Email,
            CreatedAt = DateTime.Now,
            DOB = user.DOB,
            Gender = user.Gender,
            HospitalId = hospitalId,
            Role = user.Role,
            IsFirstAppointment = false,
            Address = user.Address,
            IsActive = UserStatus.Inactive,
            PhoneNumber = user.PhoneNumber,
            UpdatedAt = DateTime.Now
        };

        try
        {
            await userDbContext.Users.AddAsync(entity);
            await userDbContext.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            UserRepositoryLogException(ex);
            throw; // preserves stack trace
        }
    }


    #region Logger Messages

    [LoggerMessage(LogLevel.Error, Message = "{exception}")]
    partial void UserRepositoryLogException(Exception exception);

    #endregion
}
