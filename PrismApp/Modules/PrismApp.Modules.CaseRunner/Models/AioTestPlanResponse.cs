using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestPlanResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("projectKey")]
        public string ProjectKey { get; set; }

        [JsonPropertyName("testCases")]
        public List<AioTestCase> TestCases { get; set; } = new();
    }
}
