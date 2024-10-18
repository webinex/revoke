namespace Webinex.Revoke;

public class RevokeArgs : RevokeId
{
    public DateTime IssuedBefore { get; }
        
    public RevokeArgs(string kind, string value, DateTime issuedBefore) : base(kind, value)
    {
        IssuedBefore = issuedBefore;
    }
}