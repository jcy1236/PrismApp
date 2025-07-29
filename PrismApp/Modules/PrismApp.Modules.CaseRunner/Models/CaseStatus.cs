using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class CaseStatus
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
