using System.Text.Json.Serialization;

namespace HealthCare.Cloud.AuthSevice.Models
{
    public class UserRegistrationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string PassWord { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public GenderType Gender { get; set; }

        [JsonPropertyName("dob")]
        public DateOnly Dob { get; set; }
    }
}
