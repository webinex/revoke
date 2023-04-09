namespace Webinex.Revoke
{
    public class RevokeId
    {
        public RevokeId(string kind, string value)
        {
            Kind = kind;
            Value = value;
        }

        public string Kind { get; }
        
        public string Value { get; }
    }
}