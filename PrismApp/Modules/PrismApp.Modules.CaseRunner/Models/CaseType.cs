using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class CaseType
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isArchived")]
        public bool IsArchived { get; set; }
    }
}
