using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webinex.Revoke.Stores
{
    /// <summary>
    ///     Revokes write store
    /// </summary>
    public interface IRevokeWriteStore
    {
        /// <summary>
        ///     Adds Revokes from <paramref name="args"/> to store.
        /// </summary>
        /// <param name="args">Revokes</param>
        Task RevokeAsync(IEnumerable<RevokeArgs> args);
    }
}