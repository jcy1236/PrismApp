using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioPagedResponse<T>
    {
        [JsonPropertyName("values")]
        public List<T> Values { get; set; } = new();

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("isLast")]
        public bool IsLast { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
