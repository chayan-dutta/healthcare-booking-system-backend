using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Cloud.AuthService.Entities
{
    /// <summary>
    /// Table
    /// </summary>
    [Table("AuthCredentials")]
    public class AuthCredential
    {
        /// <summary>
        /// Id in table
        /// </summary>
        [Key]
        [Column("Id", TypeName = "uuid")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// User id
        /// </summary>
        [Required]
        [Column("UserId", TypeName = "uuid")]
        public Guid UserId { get; set; }  // FK to UserService.Users.Id

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Column("Email", TypeName = "varchar(255)")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Password hash
        /// </summary>
        [Required]
        [Column("PasswordHash", TypeName = "text")]
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Salt password
        /// </summary>
        [Column("PasswordSalt", TypeName = "text")]
        public string? PasswordSalt { get; set; }

        /// <summary>
        /// field for checking if password is verified or not
        /// </summary>
        [Column("IsEmailVerified", TypeName = "boolean")]
        public bool IsEmailVerified { get; set; } = false;

        /// <summary>
        /// Token sent during registration for email verification
        /// </summary>
        [Column("EmailVerificationToken", TypeName = "varchar(255)")]
        public string? EmailVerificationToken { get; set; }

        /// <summary>
        /// verification token will be expired (may be 2 hrs from the registration)
        /// </summary>
        [Column("EmailVerificationExpiry", TypeName = "timestamp without time zone")]
        public DateTime? EmailVerificationExpiry { get; set; }

        /// <summary>
        /// Last login time
        /// </summary>
        [Column("LastLoginAt", TypeName = "timestamp without time zone")]
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// No of failed login ( will be used for account lock)
        /// </summary>
        [Column("FailedLoginAttempts", TypeName = "int")]
        public int FailedLoginAttempts { get; set; } = 0;

        /// <summary>
        /// Account lockout till (6 hrs locked may be for 3 unattempted login)
        /// </summary>
        [Column("LockoutUntil", TypeName = "timestamp without time zone")]
        public DateTime? LockoutUntil { get; set; }

        /// <summary>
        /// Is account locked?
        /// </summary>
        [Column("IsLocked", TypeName = "boolean")]
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// Account created at
        /// </summary>
        [Column("CreatedAt", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Account updated at
        /// </summary>
        [Column("UpdatedAt", TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// To track if it is a first login or not, the doctor has to change password
        /// </summary>
        [Column("IsFirstLogin", TypeName = "boolean")]
        public bool IsFirstLogin { get; set; } = false;
    }
}
