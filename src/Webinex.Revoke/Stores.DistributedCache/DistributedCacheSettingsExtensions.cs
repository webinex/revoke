using System;

namespace Webinex.Revoke.Stores.DistributedCache
{
    internal static class DistributedCacheSettingsExtensions
    {
        public static string Key(this IDistributedCacheSettings settings, RevokeId id)
        {
            settings = settings ?? throw new ArgumentNullException(nameof(settings));
            id = id ?? throw new ArgumentNullException(nameof(id));

            var prefix = settings.Prefix ?? string.Empty;
            return $"{prefix}{id.Kind}{settings.Separator}{id.Value}";
        }

        public static TimeSpan Expiration(this IDistributedCacheSettings settings, RevokeId id)
        {
            settings = settings ?? throw new ArgumentNullException(nameof(settings));
            id = id ?? throw new ArgumentNullException(nameof(id));
            return settings.Expiration.Invoke(id);
        }
    }
}