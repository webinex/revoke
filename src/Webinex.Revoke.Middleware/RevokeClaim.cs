using System;

namespace Webinex.Revoke.Middleware
{
    internal class RevokeClaim
    {
        public RevokeClaim(string claimName, string revokeKind)
        {
            ClaimName = claimName ?? throw new ArgumentNullException(nameof(claimName));
            RevokeKind = revokeKind ?? throw new ArgumentNullException(nameof(revokeKind));
        }

        public string ClaimName { get; }
        
        public string RevokeKind { get; }
    }
}