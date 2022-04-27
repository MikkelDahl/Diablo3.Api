namespace Diablo3.Api.Core.Models
{
    public struct Credentials
    {
        internal Credentials(string clientId, string secret)
        {
            ClientId = clientId;
            Secret = secret;
        }

        public string ClientId { get; }
        public string Secret { get; }
    }
}