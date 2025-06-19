using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestCaseResponse
    {
        [JsonPropertyName("items")]
        public List<AioTestCase> TestCases { get; set; } = new();

        [JsonPropertyName("startAt")]
        public int StartAt { get; set; }

        [JsonPropertyName("maxResults")]
        public int MaxResults { get; set; }

        [JsonPropertyName("isLast")]
        public bool IsLast { get; set; }
    }
}
