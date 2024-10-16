using ZiggyCreatures.Caching.Fusion;

namespace Webinex.Revoke.FusionCache;

internal class FusionCacheRevoke : IRevokeWriteStore, IRevokeReadStore
{
    private readonly IFusionCache _fusionCache;
    private readonly IRevokeFusionCacheSettings _settings;

    public FusionCacheRevoke(IFusionCacheProvider fusionCacheProvider, IRevokeFusionCacheSettings settings)
    {
        _settings = settings;
        _fusionCache = _settings.CacheName == null
            ? fusionCacheProvider.GetDefaultCache()
            : fusionCacheProvider.GetCache(_settings.CacheName);
    }

    public async Task<bool> RevokedAnyAsync(IEnumerable<RevokeId> ids, DateTime issued)
    {
        AssertDistributedCacheIsConfigured();

        return await RevokedAnyInternalAsync(ids, issued);
    }

    private async Task<bool> RevokedAnyInternalAsync(IEnumerable<RevokeId> ids, DateTime issued)
    {
        var tasks = ids.Select(id => RevokedAsync(id, issued));
        var remaining = new LinkedList<Task<bool>>(tasks);

        while (remaining.Count != 0)
        {
            var completed = await Task.WhenAny(remaining);
            if (completed.Result)
                return true;

            remaining.Remove(completed);
        }

        return false;
    }

    private async Task<bool> RevokedAsync(RevokeId id, DateTime issued)
    {
        var options = GetOptions(id);
        // We don't want to wait for a new value to be added to DistributedCache, so do this in background
        options.AllowBackgroundDistributedCacheOperations = true;

        var result = await _fusionCache.GetOrSetAsync<DateTime?>(
            Key(id),
            (_, _) => Task.FromResult((DateTime?)null),
            options: options);
        return result != null && issued.Ticks <= result.Value.Ticks;
    }

    public async Task RevokeAsync(IEnumerable<RevokeArgs> revokes)
    {
        AssertDistributedCacheIsConfigured();

        var tasks = revokes
            .Select(async args => { await _fusionCache.SetAsync(Key(args), args.IssuedBefore, GetOptions(args)); })
            .ToArray();

        await Task.WhenAll(tasks);
    }

    private FusionCacheEntryOptions GetOptions(RevokeId id)
    {
        var expiration = Expiration(id);

        return new FusionCacheEntryOptions
        {
            // If we have backplane we can rely on it and use the same expiration for MemoryCache
            Duration = _fusionCache.HasBackplane ? expiration : _settings.MemoryCacheExpiration(id),
            DistributedCacheDuration = expiration,
        };
    }

    private string Key(RevokeId id) => $"{_settings.Prefix}{id.Kind}{_settings.Separator}{id.Value}";
    private TimeSpan Expiration(RevokeId id) => _settings.Expiration(id);

    private void AssertDistributedCacheIsConfigured()
    {
        if (!_fusionCache.HasDistributedCache)
            throw new InvalidOperationException("Distributed cache is not configured. " +
                                                "FusionCache must have a distributed cache to properly support revoke functionality. " +
                                                "Read details here https://github.com/ZiggyCreatures/FusionCache/blob/main/docs/CacheLevels.md#-2nd-level");
    }
}