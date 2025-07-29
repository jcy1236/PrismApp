using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioBatchImportRequest
    {
        [JsonPropertyName("testCycleId")]
        public string? TestCycleId { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("fileContent")]
        public string? FileContent { get; set; }

        [JsonPropertyName("createTestCases")]
        public bool CreateTestCases { get; set; }

        [JsonPropertyName("fieldMappings")]
        public Dictionary<string, string>? FieldMappings { get; set; }
    }
}
