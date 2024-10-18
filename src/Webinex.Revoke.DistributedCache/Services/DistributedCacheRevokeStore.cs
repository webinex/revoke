using Microsoft.Extensions.Caching.Distributed;

namespace Webinex.Revoke.DistributedCache;

internal class DistributedCacheRevokeStore<TCache> : IRevokeReadStore, IRevokeWriteStore
    where TCache : IDistributedCache
{
    private readonly TCache _cache;
    private readonly IDistributedCacheSettings _settings;
    private readonly IRevokeClientCache _revokeClientCache;

    public DistributedCacheRevokeStore(
        TCache cache,
        IDistributedCacheSettings settings,
        IRevokeClientCache revokeClientCache)
    {
        _cache = cache;
        _settings = settings;
        _revokeClientCache = revokeClientCache;
    }

    public async Task<bool> RevokedAnyAsync(IEnumerable<RevokeId> idEnumerable, DateTime issuedAt)
    {
        idEnumerable = idEnumerable?.ToArray() ?? throw new ArgumentNullException(nameof(idEnumerable));

        if (idEnumerable.Any(x => x == null))
            throw new ArgumentException("Might not contain nulls.", nameof(idEnumerable));

        if (!idEnumerable.Any())
            return false;

        return await RevokedAnyInternalAsync(idEnumerable.ToArray(), issuedAt);
    }

    private async Task<bool> RevokedAnyInternalAsync(RevokeId[] ids, DateTime issued)
    {
        var tasks = ids.Select(id => RevokedAsync(id, issued)).ToArray();
        var remaining = new HashSet<Task<bool>>(tasks);

        while (remaining.Any())
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
        var key = _settings.Key(id);
        var clientCacheFound = _revokeClientCache.TryGet(key, out var clientValue);
        var revokeIssuedBeforeTicks = clientCacheFound ? clientValue : await RevokedDistributedCacheAsync(key);
        if (!clientCacheFound)
            _revokeClientCache.Store(key, revokeIssuedBeforeTicks);

        return revokeIssuedBeforeTicks.HasValue && issued.Ticks <= revokeIssuedBeforeTicks.Value;
    }

    private async Task<long?> RevokedDistributedCacheAsync(string key)
    {
        var ticksString = await _cache.GetStringAsync(key);
        return ticksString == null
            ? default(long?)
            : long.Parse(ticksString);
    }

    public async Task RevokeAsync(IEnumerable<RevokeArgs> argsEnumerable)
    {
        argsEnumerable = argsEnumerable?.ToArray() ?? throw new ArgumentNullException(nameof(argsEnumerable));

        if (argsEnumerable.Any(x => x == null))
            throw new ArgumentException("Might not contain nulls.", nameof(argsEnumerable));

        await Task.WhenAll(argsEnumerable.Select(RevokeAsync).ToArray());
    }

    private async Task RevokeAsync(RevokeArgs args)
    {
        var key = _settings.Key(args);
        var expiration = _settings.Expiration(args);
        var issuedBeforeString = args.IssuedBefore.Ticks.ToString();
        var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration };
        await _cache.SetStringAsync(key, issuedBeforeString, cacheOptions);
    }
}