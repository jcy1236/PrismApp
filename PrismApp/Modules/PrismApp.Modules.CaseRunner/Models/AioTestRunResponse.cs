using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestRunResponse
    {
        [JsonPropertyName("testruns")]
        public List<AioTestRun>? TestRuns { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }

        [JsonPropertyName("startAt")]
        public int? StartAt { get; set; }

        [JsonPropertyName("maxResults")]
        public int? MaxResults { get; set; }
    }
}
