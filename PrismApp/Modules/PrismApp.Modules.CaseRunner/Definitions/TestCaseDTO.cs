using PrismApp.Modules.CaseRunner.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismApp.Modules.CaseRunner.Definitions
{
    public class TestCaseDTO
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("precondition")]
        public string Precondition { get; set; }

        // public FolderData Folder { get; set; }
    
        // public StatusData Status { get; set; }
    
        // public PriorityData Priority { get; set; }

        [JsonPropertyName("scriptType")]
        public ScriptTypeData ScriptType { get; set; }
    
        [JsonPropertyName("type")]
        public TypeDataDto Type { get; set; }

        // public List<string> JiraComponentIDs { get; set; }

        // public List<string> JiraReleaseIDs { get; set; }

        // public int EstimatedEffort { get; set; }

        // public List<CustomField> CustomFields { get; set; }

        [JsonPropertyName("steps")]
        public List<AioTestStep> Steps { get; set; }

        // public List<DatasetParameter> DatasetParameters { get; set; }

        // public List<Dictionary<string, object>> DataSets { get; set; }

        [JsonPropertyName("tags")]
        public List<TagDataDto> Tags { get; set; }

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

        

        //public class AutomationStatusData
        //{
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public int ID { get; set; }
        //}

    

        #endregion
    }
}