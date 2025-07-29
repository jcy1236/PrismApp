using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioProjectConfig
    {
        [JsonPropertyName("caseTypes")]
        public List<CaseType>? CaseTypes { get; set; }

        [JsonPropertyName("casePriorities")]
        public List<CaseType>? CasePriorities { get; set; }

        [JsonPropertyName("caseStatuses")]
        public List<CaseStatus>? CaseStatuses { get; set; }

        [JsonPropertyName("ProjectKey")]
        public string? ProjectKey { get; set; }

        [JsonPropertyName("runStatuses")]
        public List<RunStatus>? RunStatuses { get; set; }

        [JsonPropertyName("customFields")]
        public List<AioCustomField>? CustomFields { get; set; }
    }
}
