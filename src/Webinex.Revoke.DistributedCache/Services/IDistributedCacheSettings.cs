namespace Webinex.Revoke.DistributedCache;

internal interface IDistributedCacheSettings
{
    string Prefix { get; }
    string Separator { get; }
    Func<RevokeId, TimeSpan> Expiration { get; }
    string ClientCachePrefix { get; }
    TimeSpan ClientCacheExpiration { get; }
}