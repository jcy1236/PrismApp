using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioUser
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("avatarUrls")]
        public Dictionary<string, string> AvatarUrls { get; set; } = new();
    }
}
