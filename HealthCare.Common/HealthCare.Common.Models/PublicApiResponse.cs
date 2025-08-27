using System.Net;
using System.Text.Json.Serialization;

namespace HealthCare.Common.Models;

/// <summary>
/// Common model for API response
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T> where T : notnull
{
    /// <summary>
    /// Status code to determine the status returned by API
    /// </summary>
    [JsonPropertyName("statuscode")]
    public HttpStatusCode Status { get; set; }

    /// <summary>
    /// Message if any
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Any data
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Success status
    /// </summary>
    [JsonPropertyName("isSuccessStatus")]
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Just to test
    /// </summary>
    [JsonPropertyName("otherMessage")]
    public string OtherMessage { get; set; } =string.Empty;
}
