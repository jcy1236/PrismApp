using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestCase
    {
        [JsonPropertyName("jiraProjectID")]
        public int JiraProjectID { get; set; }

        [JsonPropertyName("permission")]
        public AioPermissionDto Permission { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("precondition")]
        public string Precondition { get; set; }

        [JsonPropertyName("ownedByID")]
        public string OwnedByID { get; set; }

        [JsonPropertyName("folder")]
        public FolderDataDto Folder { get; set; }

        [JsonPropertyName("status")]
        public AioStatusDto Status { get; set; }

        [JsonPropertyName("priority")]
        public object Priority { get; set; }

        [JsonPropertyName("scriptType")]
        public AioScriptTypeDto ScriptType { get; set; }

        [JsonPropertyName("type")]
        public TypeDataDto Type { get; set; }

        [JsonPropertyName("jiraComponentIDs")]
        public List<object> JiraComponentIDs { get; set; }

        [JsonPropertyName("jiraReleaseIDs")]
        public List<object> JiraReleaseIDs { get; set; }

        [JsonPropertyName("estimatedEffort")]
        public object EstimatedEffort { get; set; }

        [JsonPropertyName("createdDate")]
        public long CreatedDate { get; set; }

        [JsonPropertyName("updatedDate")]
        public long? UpdatedDate { get; set; }

        [JsonPropertyName("isArchived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("steps")]
        public List<AioStepDto> Steps { get; set; }

        [JsonPropertyName("hasDataSets")]
        public bool HasDataSets { get; set; }

        [JsonPropertyName("tags")]
        public List<TagDataDto> Tags { get; set; }

        [JsonPropertyName("automationStatus")]
        public object AutomationStatus { get; set; }

        [JsonPropertyName("automationOwnerID")]
        public object AutomationOwnerID { get; set; }

        [JsonPropertyName("automationKey")]
        public object AutomationKey { get; set; }

        [JsonPropertyName("jiraRequirementIDs")]
        public List<object> JiraRequirementIDs { get; set; }

        [JsonPropertyName("ID")]
        public int ID { get; set; }
    }

    public class AioPermissionDto
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    public class FolderDataDto
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class AioStatusDto
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class AioScriptTypeDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }
        
        [JsonPropertyName("ID")]
        public int ID { get; set; }
    }

    public class AioStepDto
    {
        [JsonPropertyName("step")]
        public string Step { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("expectedResult")]
        public string ExpectedResult { get; set; }

        [JsonPropertyName("stepType")]
        public string StepType { get; set; }

        [JsonPropertyName("referencedCase")]
        public ReferencedCaseDto ReferencedCase { get; set; }

        [JsonPropertyName("bddStep")]
        public string BddStep { get; set; }

        [JsonPropertyName("gherkinKeyword")]
        public string GherkinKeyword { get; set; }

        [JsonPropertyName("attachments")]
        public List<AttachmentDto> Attachments { get; set; }

        [JsonPropertyName("stepAttachments")]
        public List<AttachmentDto> StepAttachments { get; set; }

        [JsonPropertyName("dataAttachments")]
        public List<AttachmentDto> DataAttachments { get; set; }

        [JsonPropertyName("expectedResultAttachments")]
        public List<AttachmentDto> ExpectedResultAttachments { get; set; }
        
        [JsonPropertyName("ID")]
        public int ID { get; set; }
    }

    public class ReferencedCaseDto
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("jiraProjectID")]
        public int JiraProjectID { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }
    }

    public class AioVersionDto
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("ID")]
        public int ID { get; set; }
    }

    public class TagDataDto
    {
        [JsonPropertyName("tag")]
        public TagDetail Tag { get; set; }

        // Inner Class
        public class TagDetail
        {
            [JsonPropertyName("ID")]
            public int ID { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }

    public class TypeDataDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isArchived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("ID")]
        public int ID { get; set; }
    }
}
