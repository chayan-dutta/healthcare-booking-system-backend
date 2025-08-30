using HealthCare.Cloud.AuthService.Models;
using HealthCare.Common.HttpClientHelper;
using HealthCare.Common.Models;
using System.Text.Json;

namespace HealthCare.Cloud.AuthService.ServiceClients;

/// <summary>
/// 
/// </summary>
public class UserServiceClient : IUserServiceClient
{
    private const string ApiPath = "/api/users/internal/createuser";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRegistrationRequest"></param>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    public async Task<ApiResponse<AddUserResponse>> AddUser(UserRegistrationRequest userRegistrationRequest, string baseUrl)
    {
        string apiUrl = $"{baseUrl}{ApiPath}";

        ClientPrimitives clientPrimitives = new()
        {
            BearerToken = "", // Place holder for making service token
            Body = userRegistrationRequest,
            Url = apiUrl
        };

        HttpResponseMessage apiResponse = await HttpClientHelper.CallHttpPostApi(clientPrimitives);

        // Read the response content
        string responseContent = await apiResponse.Content.ReadAsStringAsync();

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // Deserialize into ApiResponse<object>
        var result = JsonSerializer.Deserialize<ApiResponse<object>>(
            responseContent,
            options: options);

        // If deserialization fails, create a fallback response
        if (result == null)
        {
            return new ApiResponse<object>
            {
                IsSuccess = false,
                Status = apiResponse.StatusCode,
                Message = "Failed to parse response",
                Data = null
            };
        }

        return result;
    }
}
