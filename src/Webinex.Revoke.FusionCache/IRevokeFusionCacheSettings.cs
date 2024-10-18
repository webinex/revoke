namespace Webinex.Revoke.FusionCache;

internal interface IRevokeFusionCacheSettings
{
    string? CacheName { get; }
    string Prefix { get; }
    string Separator { get; }
    Func<RevokeId, TimeSpan> MemoryCacheExpiration { get; }
    Func<RevokeId, TimeSpan> Expiration { get; }
}