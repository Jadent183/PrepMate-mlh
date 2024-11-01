using Microsoft.JSInterop;
using System.Text.Json;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<LocalStorageService> _logger;

    public LocalStorageService(IJSRuntime jsRuntime, ILogger<LocalStorageService> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public async Task<T> GetItemAsync<T>(string key)
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Unable to access localStorage due to prerendering.");
            return default;
        }
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Unable to access localStorage due to prerendering.");
        }
    }

    public async Task RemoveItemAsync(string key)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Unable to access localStorage due to prerendering.");
        }
    }
}