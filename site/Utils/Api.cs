using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace site.Utils;

/// <summary>
/// Reusable HTTP API client with GET, POST, PUT, PATCH, DELETE methods.
/// Reads base URL from configuration. Falls back to a default if not set.
/// 
/// Usage:
///   var api = new Api(httpClient);                    // uses default base URL
///   var api = new Api(httpClient, "https://custom");  // custom base URL
///   
///   var data = await api.GetAsync&lt;MyModel&gt;("/endpoint");
///   var result = await api.PostAsync&lt;MyModel&gt;("/endpoint", payload);
/// </summary>
public class Api
{
    private const string DefaultBaseUrl = "https://api.github.com";

    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Creates a new API client.
    /// </summary>
    /// <param name="http">The HttpClient instance (injected via DI)</param>
    /// <param name="baseUrl">Optional base URL. If null/empty, uses the default from config.</param>
    public Api(HttpClient http, string? baseUrl = null)
    {
        _http = http;
        _baseUrl = (baseUrl ?? DefaultBaseUrl).TrimEnd('/');
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        // Set default headers
        if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _http.DefaultRequestHeaders.Add("User-Agent", "SharpIB-Site");
        }
    }

    /// <summary>Base URL this client is configured with.</summary>
    public string BaseUrl => _baseUrl;

    // ==================== GET ====================

    /// <summary>Sends a GET request and deserializes the JSON response.</summary>
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var url = BuildUrl(endpoint);
            return await _http.GetFromJsonAsync<T>(url, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API GET ERROR] {endpoint}: {ex.Message}");
            return default;
        }
    }

    /// <summary>Sends a GET request and returns the raw response string.</summary>
    public async Task<string?> GetStringAsync(string endpoint)
    {
        try
        {
            var url = BuildUrl(endpoint);
            return await _http.GetStringAsync(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API GET ERROR] {endpoint}: {ex.Message}");
            return null;
        }
    }

    // ==================== POST ====================

    /// <summary>Sends a POST request with a JSON payload and deserializes the response.</summary>
    public async Task<TResponse?> PostAsync<TResponse>(string endpoint, object? payload = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var response = await _http.PostAsJsonAsync(url, payload, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API POST ERROR] {endpoint}: {ex.Message}");
            return default;
        }
    }

    /// <summary>Sends a POST request with a JSON payload. Returns true if successful.</summary>
    public async Task<bool> PostAsync(string endpoint, object? payload = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var response = await _http.PostAsJsonAsync(url, payload, _jsonOptions);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API POST ERROR] {endpoint}: {ex.Message}");
            return false;
        }
    }

    // ==================== PUT ====================

    /// <summary>Sends a PUT request with a JSON payload and deserializes the response.</summary>
    public async Task<TResponse?> PutAsync<TResponse>(string endpoint, object? payload = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var response = await _http.PutAsJsonAsync(url, payload, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API PUT ERROR] {endpoint}: {ex.Message}");
            return default;
        }
    }

    /// <summary>Sends a PUT request. Returns true if successful.</summary>
    public async Task<bool> PutAsync(string endpoint, object? payload = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var response = await _http.PutAsJsonAsync(url, payload, _jsonOptions);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API PUT ERROR] {endpoint}: {ex.Message}");
            return false;
        }
    }

    // ==================== PATCH ====================

    /// <summary>Sends a PATCH request with a JSON payload and deserializes the response.</summary>
    public async Task<TResponse?> PatchAsync<TResponse>(string endpoint, object? payload = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var content = new StringContent(
                JsonSerializer.Serialize(payload, _jsonOptions),
                Encoding.UTF8, "application/json");
            var response = await _http.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API PATCH ERROR] {endpoint}: {ex.Message}");
            return default;
        }
    }

    /// <summary>Sends a PATCH request. Returns true if successful.</summary>
    public async Task<bool> PatchAsync(string endpoint, object? payload = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var content = new StringContent(
                JsonSerializer.Serialize(payload, _jsonOptions),
                Encoding.UTF8, "application/json");
            var response = await _http.PatchAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API PATCH ERROR] {endpoint}: {ex.Message}");
            return false;
        }
    }

    // ==================== DELETE ====================

    /// <summary>Sends a DELETE request and deserializes the response.</summary>
    public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var response = await _http.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API DELETE ERROR] {endpoint}: {ex.Message}");
            return default;
        }
    }

    /// <summary>Sends a DELETE request. Returns true if successful.</summary>
    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var url = BuildUrl(endpoint);
            var response = await _http.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API DELETE ERROR] {endpoint}: {ex.Message}");
            return false;
        }
    }

    // ==================== HELPERS ====================

    /// <summary>Builds the full URL from base + endpoint. If the endpoint is a full URL, uses it directly.</summary>
    private string BuildUrl(string endpoint)
    {
        if (endpoint.StartsWith("http://") || endpoint.StartsWith("https://"))
            return endpoint;

        return $"{_baseUrl}/{endpoint.TrimStart('/')}";
    }
}

