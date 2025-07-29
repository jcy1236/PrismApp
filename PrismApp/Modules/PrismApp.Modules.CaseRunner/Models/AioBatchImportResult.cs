using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioBatchImportResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("processedCount")]
        public int ProcessedCount { get; set; }

        [JsonPropertyName("successCount")]
        public int SuccessCount { get; set; }

        [JsonPropertyName("failureCount")]
        public int FailureCount { get; set; }

        [JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }

        [JsonPropertyName("createdTestCases")]
        public List<string>? CreatedTestCases { get; set; }

        [JsonPropertyName("updatedTestRuns")]
        public List<string>? UpdatedTestRuns { get; set; }
    }
}
