using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestRun
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("testCaseKey")]
        public string? TestCaseKey { get; set; }

        [JsonPropertyName("testCycleId")]
        public string? TestCycleId { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("executedDate")]
        public DateTime? ExecutedDate { get; set; }

        [JsonPropertyName("executedBy")]
        public string? ExecutedBy { get; set; }

        [JsonPropertyName("executionTime")]
        public TimeSpan? ExecutionTime { get; set; }

        [JsonPropertyName("environment")]
        public string? Environment { get; set; }

        [JsonPropertyName("attachments")]
        public List<AttachmentDto>? Attachments { get; set; }
    }
}
