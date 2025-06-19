using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class ReferencedCaseData
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("jiraProjectID")]
        public int JiraProjectID { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }
    }
}
