namespace Webinex.Revoke.Middleware;

public static class RevokeMiddlewareConfigurationExtensions
{
    /// <summary>
    ///     Adds claim to check for revoke. Revoke kind of <paramref name="name"/> would be passed
    ///     to revoke check with value of <paramref name="name"/> claim.
    /// 
    ///     Note: it would be ignored if you'll register your own implementation of <see cref="IRevokeAccessor"/>
    /// </summary>
    /// <param name="configuration"><see cref="IRevokeMiddlewareConfiguration"/></param>
    /// <param name="name">Claim and Revoke kind</param>
    public static IRevokeMiddlewareConfiguration UseClaim(
        this IRevokeMiddlewareConfiguration configuration,
        string name)
    {
        configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        name = name ?? throw new ArgumentNullException(nameof(name));

        return configuration.UseClaim(name, name);
    }
}