using System;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestCycleSummary
    {
        [JsonPropertyName("cycleId")]
        public string? CycleId { get; set; }

        [JsonPropertyName("cycleName")]
        public string? CycleName { get; set; }

        [JsonPropertyName("totalTestCases")]
        public int TotalTestCases { get; set; }

        [JsonPropertyName("passedCount")]
        public int PassedCount { get; set; }

        [JsonPropertyName("failedCount")]
        public int FailedCount { get; set; }

        [JsonPropertyName("blockedCount")]
        public int BlockedCount { get; set; }

        [JsonPropertyName("notExecutedCount")]
        public int NotExecutedCount { get; set; }

        [JsonPropertyName("progressPercentage")]
        public double ProgressPercentage { get; set; }

        [JsonPropertyName("totalExecutionTime")]
        public TimeSpan? TotalExecutionTime { get; set; }
    }
}
