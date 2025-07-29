using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AttachmentDto
    {
        [JsonPropertyName("ID")]
        public int? ID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        [JsonPropertyName("size")]
        public long? Size { get; set; }

        [JsonPropertyName("checksum")]
        public string? Checksum { get; set; }

        [JsonPropertyName("ownerId")]
        public string? OwnerId { get; set; }

        [JsonPropertyName("createdDate")]
        public long? CreatedDate { get; set; }

        //[JsonPropertyName("fileName")]
        //public string? FileName { get; set; }

        // [JsonPropertyName("content")]
        public string? Content { get; set; }

        // [JsonPropertyName("isContentLoaded")]
        public bool IsContentLoaded { get; set; }

        //[JsonPropertyName("author")]
        //public string? Author { get; set; }
    }
}
