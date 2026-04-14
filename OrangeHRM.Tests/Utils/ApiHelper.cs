using System.Text.Json;

namespace OrangeHRM.Tests.Utils;

public static class ApiHelper
{
    private static readonly HttpClient HttpClient = new();

    public const string JsonPlaceholderUsersUrl = "https://jsonplaceholder.typicode.com/users";

    /// <summary>
    /// Sends a GET request to the given URL and returns the response.
    /// </summary>
    public static async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        return await HttpClient.GetAsync(url, cancellationToken);
    }

    /// <summary>
    /// Sample: GET https://jsonplaceholder.typicode.com/users
    /// Returns response and parsed JSON array. Caller validates status and content.
    /// </summary>
    public static async Task<(HttpResponseMessage Response, JsonElement[] Users)> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetAsync(JsonPlaceholderUsersUrl, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        var users = root.ValueKind == JsonValueKind.Array
            ? root.EnumerateArray().ToArray()
            : Array.Empty<JsonElement>();
        return (response, users);
    }
}
