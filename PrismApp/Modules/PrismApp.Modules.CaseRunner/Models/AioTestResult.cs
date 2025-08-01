﻿using System;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestResult
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("executedAt")]
        public DateTime ExecutedAt { get; set; }

        [JsonPropertyName("executedBy")]
        public string? ExecutedBy { get; set; }

        [JsonPropertyName("executionTime")]
        public TimeSpan? ExecutionTime { get; set; }

        [JsonPropertyName("environment")]
        public string? Environment { get; set; }
    }
}
