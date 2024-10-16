namespace Webinex.Revoke.DistributedCache.Services;

internal interface IDistributedCacheSettings
{
    string Prefix { get; }
    string Separator { get; }
    Func<RevokeId, TimeSpan> Expiration { get; }
    string ClientCachePrefix { get; }
    TimeSpan ClientCacheExpiration { get; }
}