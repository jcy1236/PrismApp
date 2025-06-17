using System.Collections.Generic;
using System.Text.Json.Serialization;

public class TestCaseDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("precondition")]
    public string Precondition { get; set; } = string.Empty;

    // public FolderData Folder { get; set; }
    
    // public StatusData Status { get; set; }
    
    // public PriorityData Priority { get; set; }

    [JsonPropertyName("scriptType")]
    public ScriptTypeData ScriptType { get; set; }
    
    [JsonPropertyName("type")]
    public TypeData Type{ get; set; }

    // public List<string> JiraComponentIDs { get; set; }

    // public List<string> JiraReleaseIDs { get; set; }

    // public int EstimatedEffort { get; set; }

    // public List<CustomField> CustomFields { get; set; }

    [JsonPropertyName("steps")]
    public List<TestStep> Steps { get; set; }

    // public List<DatasetParameter> DatasetParameters { get; set; }

    // public List<Dictionary<string, object>> DataSets { get; set; }

    [JsonPropertyName("tags")]
    public List<TagData> Tags { get; set; }

    // public AutomationStatusData AutomationStatus { get; set; }

    // public string AutomationOwnerID { get; set; }

    // public string AutomationKey { get; set; }

    // public List<string> JiraRequirementIDs { get; set; }

    [JsonPropertyName("ID")]
    public int ID { get; set; }


    #region Inner Class

    // Inner Class
    //public class FolderData
    //{
    //    public int ID { get; set; }
    //}

    //public class StatusData
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public int ID { get; set; }
    //}

    //public class  PriorityData
    //{
    //    public string Name { get; set; }
    //    public int ID { get; set; }
    //}

    public class ScriptTypeData
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }
    }

    public class TypeData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        // public bool IsArchived { get; set; }
        // public int ID { get; set; }
    }

    //public class CustomField
    //{
    //    public string Name { get; set; }
    //    public object Value { get; set; }
    //    public int ID { get; set; }
    //}



    //public class DataSetParameter
    //{
    //    public string Name { get; set; }
    //    public int PredefinedParameterID { get; set; }
    //    public string PredefinedParameterName { get; set; }
    //    public int ID { get; set; }
    //}

    public class TagData
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

    public class AutomationStatusData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
    }

    public class TestStep
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("step")]
        public string Step { get; set; }

        //[JsonPropertyName("data")]
        //public string Data { get; set; }

        [JsonPropertyName("expectedResult")]
        public string ExpectedResult { get; set; }

        [JsonPropertyName("stepType")]
        public string StepType { get; set; }

        //[JsonPropertyName("referencedCase")]
        //public ReferencedCaseData ReferencedCase { get; set; }

        //[JsonPropertyName("bddStep")]
        //public string BddStep { get; set; }

        //[JsonPropertyName("GherkinKeyword")]
        //public string GherkinKeyword { get; set; }


        // Inner Class
        public class ReferencedCaseData
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
    }

    #endregion
}