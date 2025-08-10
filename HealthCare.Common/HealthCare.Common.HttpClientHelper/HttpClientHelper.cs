using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HealthCare.Common.HttpClientHelper;

/// <summary>
/// 
/// </summary>
public static class HttpClientHelper
{
    private static HttpClient _httpClient = new();

    /// <summary>
    /// Helper method for Get API call
    /// </summary>
    /// <param name="primitives"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> CallHttpGetApi(ClientPrimitives primitives)
    {
        var requestUrl = BuildUrlWithQuery(primitives.Url, primitives.QueryParams);
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        AddHeaders(request, primitives);
        return await _httpClient.SendAsync(request);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="primitives"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> CallHttpPostApi(ClientPrimitives primitives)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, primitives.Url);
        AddHeaders(request, primitives);

        if (primitives.Body != null)
        {
            var json = JsonSerializer.Serialize(primitives.Body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return await _httpClient.SendAsync(request);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="primitives"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> CallHttpPutApi(ClientPrimitives primitives)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, primitives.Url);
        AddHeaders(request, primitives);

        if (primitives.Body != null)
        {
            var json = JsonSerializer.Serialize(primitives.Body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return await _httpClient.SendAsync(request);
    }

    /// <summary>
    /// Patch API
    /// </summary>
    /// <param name="primitives"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> CallHttpPatchApi(ClientPrimitives primitives)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, primitives.Url);
        AddHeaders(request, primitives);

        if (primitives.Body != null)
        {
            var json = JsonSerializer.Serialize(primitives.Body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return await _httpClient.SendAsync(request);
    }

    /// <summary>
    /// Delete API
    /// </summary>
    /// <param name="primitives"></param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> CallHttpDeleteApi(ClientPrimitives primitives)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, primitives.Url);
        AddHeaders(request, primitives);
        return await _httpClient.SendAsync(request);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    private static string BuildUrlWithQuery(string url, Dictionary<string, string>? queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
            return url;

        var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{url}?{query}";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="primitives"></param>
    private static void AddHeaders(HttpRequestMessage request, ClientPrimitives primitives)
    {
        // Default Headers
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*")); // Accept: */*

        // Authorization
        if (!string.IsNullOrEmpty(primitives.BearerToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", primitives.BearerToken);

        // Custom Headers
        if (primitives.Headers != null)
        {
            foreach (var header in primitives.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
