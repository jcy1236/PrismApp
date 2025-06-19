using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestScript
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } // "STEP_BY_STEP", "PLAIN_TEXT", "BDD"

        [JsonPropertyName("steps")]
        public List<AioTestStep> Steps { get; set; } = new();

        [JsonPropertyName("plainText")]
        public string PlainText { get; set; }
    }
}
