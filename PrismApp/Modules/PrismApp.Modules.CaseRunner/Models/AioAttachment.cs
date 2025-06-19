using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioAttachment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
