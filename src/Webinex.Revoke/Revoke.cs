namespace Webinex.Revoke;

internal class Revoke : IRevoke
{
    private readonly IRevokeWriteStore _revokeWriteStore;
    private readonly IRevokeReadStore _revokeReadStore;

    public Revoke(IRevokeWriteStore revokeWriteStore, IRevokeReadStore revokeReadStore)
    {
        _revokeWriteStore = revokeWriteStore;
        _revokeReadStore = revokeReadStore;
    }

    public async Task<bool> RevokedAnyAsync(IEnumerable<RevokeId> ids, DateTime issued)
    {
        return await _revokeReadStore.RevokedAnyAsync(ids, issued);
    }

    public async Task RevokeAsync(IEnumerable<RevokeArgs> revokes)
    {
        await _revokeWriteStore.RevokeAsync(revokes);
    }
}