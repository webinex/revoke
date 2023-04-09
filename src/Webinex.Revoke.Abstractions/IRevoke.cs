using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webinex.Revoke
{
    public interface IRevoke
    {
        /// <summary>
        ///     Returns <c>True</c> if store contains any entry for <paramref name="ids"/> with
        ///     revoke for things issued earlier than <paramref name="issued"/>
        /// </summary>
        /// <param name="ids">Revokes to check</param>
        /// <param name="issued">Revokes might be greater than or equal to it</param>
        Task<bool> RevokedAnyAsync(IEnumerable<RevokeId> ids, DateTime issued);

        /// <summary>
        ///     Adds given entries to revoke store
        /// </summary>
        /// <param name="revokes">Revokes to store</param>
        Task RevokeAsync(IEnumerable<RevokeArgs> revokes);
    }
}