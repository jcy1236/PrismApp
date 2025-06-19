using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioStepResult
    {
        [JsonPropertyName("stepId")]
        public int StepId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("actualResult")]
        public string ActualResult { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}
