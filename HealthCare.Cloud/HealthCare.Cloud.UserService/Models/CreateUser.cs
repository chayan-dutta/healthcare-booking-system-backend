using System.Text.Json.Serialization;

namespace HealthCare.Cloud.UserService.Models
{
    public class CreateUser
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("full_name")]
        public required string FullName { get; set; }

        [JsonPropertyName("role")]
        public UserRole Role { get; set; }

        [JsonPropertyName("hospital_id")]
        public Guid HospitalId { get; set; } = Guid.Empty;

        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("gender")]
        public Gender Gender { get; set; }

        [JsonPropertyName("dob")]
        public DateTime DOB { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }
    }
}
