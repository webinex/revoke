using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Webinex.Revoke.Middleware;

internal class RevokeMiddlewareConfiguration : IRevokeMiddlewareSettings, IRevokeMiddlewareConfiguration
{
    private readonly List<RevokeClaim> _claims = new List<RevokeClaim>();
    private readonly IServiceCollection _services;

    public RevokeMiddlewareConfiguration(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));

        _services.AddSingleton<IRevokeMiddlewareSettings>(this);
    }

    public string RevokedHeaderName { get; private set; } = "X-REVOKED";
    public RevokeClaim[] Claims => _claims.ToArray();
    public string IssuedAtClaimName { get; private set; } = "iat";

    public IRevokeMiddlewareConfiguration UseRevokedHeaderName(string value)
    {
        RevokedHeaderName = value ?? throw new ArgumentNullException(nameof(value));
        return this;
    }

    public IRevokeMiddlewareConfiguration UseUnixTimeIssuedAtClaimName(string name)
    {
        IssuedAtClaimName = name ?? throw new ArgumentNullException(nameof(name));
        return this;
    }

    public IRevokeMiddlewareConfiguration UseClaim(string claim, string kind)
    {
        claim = claim ?? throw new ArgumentNullException(nameof(claim));
        kind = kind ?? throw new ArgumentNullException(nameof(kind));

        _claims.Add(new RevokeClaim(claim, kind));
        return this;
    }

    internal void Complete()
    {
        _services.TryAddSingleton<IRevokeAccessor>(new DelegateRevokeAccessor(httpContext =>
        {
            return _claims.Select(revoke => (revoke, claims: httpContext.User.FindAll(revoke.ClaimName)))
                .SelectMany(tuple =>
                    tuple.claims.Select(claim => new RevokeId(tuple.revoke.RevokeKind, claim.Value))).ToArray();
        }));
        
        _services.TryAddSingleton<IRevokeIssuedAtAccessor>(new DelegateIssuedAtAccessor(httpContext =>
        {
            var value = httpContext.User.FindFirst(IssuedAtClaimName)?.Value ??
                        throw new InvalidOperationException($"Issued claim ({IssuedAtClaimName}) not found");
            var longValue = long.Parse(value);
            return DateTimeOffset.FromUnixTimeSeconds(longValue).UtcDateTime;
        }));
    }
}