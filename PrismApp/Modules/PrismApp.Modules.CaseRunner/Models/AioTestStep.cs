using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestStep    //AioStepDto
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("orderIndex")]
        public int OrderIndex { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("expectedResult")]
        public string ExpectedResult { get; set; }

        [JsonPropertyName("attachments")]
        public List<AioAttachment> Attachments { get; set; } = new();

        [JsonPropertyName("step")]
        public string Step { get; set; }

        [JsonPropertyName("stepType")]
        public string StepType { get; set; }

        [JsonPropertyName("referencedCase")]
        public ReferencedCaseData ReferencedCase { get; set; }

        [JsonPropertyName("bddStep")]
        public string BddStep { get; set; }

        [JsonPropertyName("GherkinKeyword")]
        public string GherkinKeyword { get; set; }

        [JsonPropertyName("parameters")]
        public Dictionary<string, object> Parameters { get; set; }

        [JsonPropertyName("timeout")]
        public TimeSpan? Timeout { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("action")]
        public int Order { get; set; }
    }
}
