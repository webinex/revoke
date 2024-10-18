namespace Webinex.Revoke.Middleware;

internal interface IRevokeMiddlewareSettings
{
    string RevokedHeaderName { get; }
}