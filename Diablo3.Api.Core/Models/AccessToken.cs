using Newtonsoft.Json;

namespace Diablo3.Api.Core.Models
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }
        [JsonProperty("token_type")]
        public string Type { get; set; }
        [JsonProperty("expires_in")]
        public int Expiration { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
};

