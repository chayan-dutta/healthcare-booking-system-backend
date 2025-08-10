using System.Net;

namespace HealthCare.Common.Models;

/// <summary>
/// Common model for API response
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T> where T : notnull
{
    /// <summary>
    /// Status code
    /// </summary>
    public HttpStatusCode Status { get; set; }

    /// <summary>
    /// Any data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Success status
    /// </summary>
    public bool IsSuccess { get; set; }
}
