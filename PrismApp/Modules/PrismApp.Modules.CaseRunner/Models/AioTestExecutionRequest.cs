using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestExecutionRequest
    {
        [JsonPropertyName("testCaseKey")]
        public string TestCaseKey { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("executedOn")]
        public string ExecutedOn { get; set; }

        [JsonPropertyName("executedBy")]
        public string ExecutedBy { get; set; }

        [JsonPropertyName("stepResults")]
        public List<AioStepResult> StepResults { get; set; } = new();
    }
}
