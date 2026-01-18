namespace Mobilis_Real_Time_Assistant.Service;

public interface IContextCacheService
{
    Task<string> GetContextAsync();
    Task ReloadContextAsync();
    void ClearCache();
}

public class ContextCacheService : IContextCacheService
{
    private readonly string _contextFilePath;

    private string? _cachedContext;

    private DateTime? _lastLoadTime;

    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(30);

    private readonly SemaphoreSlim _cacheLock = new(1, 1);

    public ContextCacheService()
    {
        // load context file path
        _contextFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "ContextResponse.txt");
    }

    // method to get cached context or load it from file
    public async Task<string> GetContextAsync()
    {
        // check if cache is still valid
        if (_cachedContext != null && 
            _lastLoadTime.HasValue && 
            DateTime.Now - _lastLoadTime.Value < _cacheExpiry)
        {
            return _cachedContext;
        }

        await _cacheLock.WaitAsync();
        try
        {
            if (_cachedContext != null && 
                _lastLoadTime.HasValue && 
                DateTime.Now - _lastLoadTime.Value < _cacheExpiry)
            {
                return _cachedContext;
            }

            var contextContent = await File.ReadAllTextAsync(_contextFilePath);

            if (contextContent.Length > 50000)
            {
                contextContent = contextContent.Substring(0, 50000) + "\n\n[Context truncated due to length...]";
            }

            // update cache
            _cachedContext = contextContent;
            _lastLoadTime = DateTime.Now;

            return _cachedContext;
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    public async Task ReloadContextAsync()
    {
        await _cacheLock.WaitAsync();
        try
        {
            _cachedContext = null;
            _lastLoadTime = null;

            await GetContextAsync();
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    // method to clear cache without reloading
    public void ClearCache()
    {
        _cacheLock.Wait();
        try
        {
            _cachedContext = null;
            _lastLoadTime = null;
        }
        finally
        {
            _cacheLock.Release();
        }
    }
}