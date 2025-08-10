using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Cloud.UserService.Models;

/// <summary>
/// 
/// </summary>
[Table("Users")]
public class User
{
    /// <summary>
    /// 
    /// </summary>
    [Key]
    [Column("Id", TypeName = "uuid")]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Column("Email", TypeName = "varchar(255)")]
    public required string Email { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Column("FullName", TypeName = "varchar(255)")]
    public required string FullName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Column("Role", TypeName = "int")] 
    public UserRole Role { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("HospitalId", TypeName = "uuid")]
    public Guid? HospitalId { get; set; } = Guid.Empty;

    /// <summary>
    /// 
    /// </summary>
    [Column("PhoneNumber", TypeName = "varchar(20)")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("IsFirstAppointment", TypeName = "boolean")]
    public bool IsFirstAppointment { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    [Column("CreatedAt", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Column("Gender", TypeName = "int")] 
    public Gender Gender { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [Column("DOB", TypeName = "date")]
    public DateTime DOB { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("Address", TypeName = "jsonb")]
    public Address? Address { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("UpdatedAt", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Column("IsActive", TypeName = "int")]
    public UserStatus IsActive { get; set; } = UserStatus.Inactive;
}
