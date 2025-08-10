namespace HealthCare.Common.HttpClientHelper;

public class ClientPrimitives
{
    /// <summary>Full API endpoint URL</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Optional headers</summary>
    public Dictionary<string, string> Headers { get; set; } = [];

    /// <summary>Bearer token for Authorization</summary>
    public string? BearerToken { get; set; }

    /// <summary>Request body object (will be serialized to JSON)</summary>
    public object? Body { get; set; }

    /// <summary>Query parameters</summary>
    public Dictionary<string, string>? QueryParams { get; set; }

}
