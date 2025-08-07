using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Cloud.AuthSevice.Entities
{
    [Table("AuthCredentials")]
    public class AuthCredential
    {
        /// <summary>
        /// Id in table
        /// </summary>
        [Key]
        [Column("Id", TypeName = "uuid")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("UserId", TypeName = "uuid")]
        public Guid UserId { get; set; }  // FK to UserService.Users.Id

        [Required]
        [Column("Email", TypeName = "varchar(255)")]
        public string Email { get; set; } = null!;

        [Required]
        [Column("PasswordHash", TypeName = "text")]
        public string PasswordHash { get; set; } = null!;

        [Column("PasswordSalt", TypeName = "text")]
        public string? PasswordSalt { get; set; }

        [Column("IsEmailVerified", TypeName = "boolean")]
        public bool IsEmailVerified { get; set; } = false;

        [Column("EmailVerificationToken", TypeName = "varchar(255)")]
        public string? EmailVerificationToken { get; set; }

        [Column("EmailVerificationExpiry", TypeName = "timestamp without time zone")]
        public DateTime? EmailVerificationExpiry { get; set; }

        [Column("LastLoginAt", TypeName = "timestamp without time zone")]
        public DateTime? LastLoginAt { get; set; }

        [Column("FailedLoginAttempts", TypeName = "int")]
        public int FailedLoginAttempts { get; set; } = 0;

        [Column("LockoutUntil", TypeName = "timestamp without time zone")]
        public DateTime? LockoutUntil { get; set; }

        [Column("IsLocked", TypeName = "boolean")]
        public bool IsLocked { get; set; } = false;

        [Column("CreatedAt", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt", TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; }
    }
}
