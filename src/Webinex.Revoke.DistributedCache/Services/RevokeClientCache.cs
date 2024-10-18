using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Webinex.Revoke.DistributedCache;

internal interface IRevokeClientCache
{
    bool TryGet(string key, out long? value);

    void Store(string key, long? value);
}

internal class EmptyRevokeClientCache : IRevokeClientCache
{
    public bool TryGet(string key, out long? value)
    {
        value = null;
        return false;
    }

    public void Store(string key, long? value)
    {
    }
}

internal class RevokeClientCache : IRevokeClientCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCacheSettings _settings;
    private readonly ILogger<RevokeClientCache> _logger;

    public RevokeClientCache(IMemoryCache memoryCache, IDistributedCacheSettings settings,
        ILogger<RevokeClientCache> logger)
    {
        _memoryCache = memoryCache;
        _settings = settings;
        _logger = logger;
    }

    public bool TryGet(string key, out long? value)
    {
        key = key ?? throw new ArgumentNullException(nameof(key));
        key = _settings.ClientCachePrefix + key;

        var found = _memoryCache.TryGetValue(key, out value);
        LogFound(key, found);
        return found;
    }

    private void LogFound(string key, bool found)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            var foundPart = found ? "found" : "not found";
            var message = $"Cache key {key} {foundPart} in a client cache.";
            _logger.LogTrace(message);
        }
    }

    public void Store(string key, long? value)
    {
        key = key ?? throw new ArgumentNullException(nameof(key));
        key = _settings.ClientCachePrefix + key;
        _memoryCache.Set(key, value, _settings.ClientCacheExpiration);
    }
}