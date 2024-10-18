namespace Webinex.Revoke.Middleware;

public interface IRevokeMiddlewareConfiguration
{
    /// <summary>
    ///     Revoke header name.
    ///     Default: "X-REVOKED"
    /// </summary>
    /// <param name="value">Revoked header name</param>
    IRevokeMiddlewareConfiguration UseRevokedHeaderName(string value);
        
    /// <summary>
    ///     Issued at claim name with Unix time value.
    ///     Default: "iat"
    ///
    ///     Note: it would be ignored if you'll register your own implementation of <see cref="IRevokeIssuedAtAccessor"/>
    /// </summary>
    /// <param name="name">Claim name to be used</param>
    IRevokeMiddlewareConfiguration UseUnixTimeIssuedAtClaimName(string name);

    /// <summary>
    ///     Adds claim to check for revoke. <paramref name="kind"/> would be passed
    ///     to revoke check with value of <paramref name="claim"/>.
    /// 
    ///     Note: it would be ignored if you'll register your own implementation of <see cref="IRevokeAccessor"/>
    /// </summary>
    /// <param name="claim">Claim to get value from</param>
    /// <param name="kind">Revoke kind to check</param>
    IRevokeMiddlewareConfiguration UseClaim(string claim, string kind);
}