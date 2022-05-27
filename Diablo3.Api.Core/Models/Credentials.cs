namespace Diablo3.Api.Core.Models
{
    internal struct Credentials
    {
        public Credentials(string clientId, string secret)
        {
            if (string.IsNullOrWhiteSpace(clientId)) 
                throw new ArgumentNullException(nameof(clientId));
            
            if (string.IsNullOrWhiteSpace(secret)) 
                throw new ArgumentNullException(nameof(secret));

            ClientId = clientId;
            Secret = secret;
        }

        public string ClientId { get; }
        public string Secret { get; }
    }
}