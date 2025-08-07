using HealthCare.Cloud.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Cloud.UserService.Data;

/// <summary>
/// 
/// </summary>
public class UserDbContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextOptions"></param>
    public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions)
        : base (dbContextOptions)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique index on Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

    }
}
