namespace Webinex.Revoke;

public static class ProtegoRevokeExtensions
{
    /// <summary>
    ///     Revokes <paramref name="kind"/> with <paramref name="value"/> issued earlier current moment.
    /// </summary>
    /// <example>
    ///     _revoke.RevokeAsync("id", user.Id);
    /// </example>
    /// <param name="revoke">Service</param>
    /// <param name="kind">Revoke entry kind</param>
    /// <param name="value">Revoke entry value</param>
    public static async Task RevokeAsync(this IRevoke revoke, string kind, string value)
    {
        revoke = revoke ?? throw new ArgumentNullException(nameof(revoke));
        kind = kind ?? throw new ArgumentNullException(nameof(kind));
        value = value ?? throw new ArgumentNullException(nameof(value));

        await RevokeAsync(revoke, kind, value, DateTime.UtcNow);
    }

    /// <summary>
    ///     Revokes <paramref name="kind"/> with <paramref name="value"/> issued
    ///     earlier than <paramref name="issuedBefore"/>.
    /// </summary>
    /// <example>
    ///     _revoke.RevokeAsync("id", user.Id, DateTime.UtcNow);
    /// </example>
    /// <param name="revoke">Service</param>
    /// <param name="kind">Revoke entry kind</param>
    /// <param name="value">Revoke entry value</param>
    /// <param name="issuedBefore">Things issued earlier than this value will be revoked</param>
    public static async Task RevokeAsync(this IRevoke revoke, string kind, string value, DateTime issuedBefore)
    {
        revoke = revoke ?? throw new ArgumentNullException(nameof(revoke));
        kind = kind ?? throw new ArgumentNullException(nameof(kind));
        value = value ?? throw new ArgumentNullException(nameof(value));

        var args = new RevokeArgs(kind, value, issuedBefore);
        await RevokeAsync(revoke, args);
    }

    /// <summary>
    ///     Revokes things matching <paramref name="args"/>
    /// </summary>
    /// <example>
    ///     var args = new RevokeArgs("id", user.Id, DateTime.UtcNow);
    ///     _revoke.RevokeAsync(args);
    /// </example>
    /// <param name="revoke">Service</param>
    /// <param name="args">Describes what things might be revoked</param>
    public static async Task RevokeAsync(this IRevoke revoke, RevokeArgs args)
    {
        revoke = revoke ?? throw new ArgumentNullException(nameof(revoke));
        args = args ?? throw new ArgumentNullException(nameof(args));

        await revoke.RevokeAsync(new[] { args });
    }
}