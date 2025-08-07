using HealthCare.Cloud.AuthSevice.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Cloud.AuthSevice.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthServiceDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base (options)
        {  }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<AuthCredential> AuthCredentials { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthCredential>()
                .HasIndex(i => i.Email)
                .IsUnique();

            modelBuilder.Entity<AuthCredential>()
                .HasIndex(i => i.UserId)
                .IsUnique();
        }
    }
}
