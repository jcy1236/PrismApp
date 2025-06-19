using PrismApp.Modules.CaseRunner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Interfaces
{
    public interface ITestCaseManager
    {
        public Task<string> GetTestCaseDetailAsync(string jiraProjectId, string testCaseId, bool needDataInRTF = false, bool needAttachments = false, int version = 1);
        
        public Task<bool> UploadAttachmentToTestCaseAsync(string jiraProjectId, string testCaseId, string filePath, int version = 1);
        
        public Task<bool> UploadAttachmentToTestCaseRTFFieldAsync(string jiraProjectId, string testCaseId, string fieldId, string filePath, int version = 1);
        
        public Task<string?> CreateTestCaseAsync(string jiraProjectId, object requestBody, bool needDataInRTF = false, bool uniqueAutokey = false);

        public Task<IEnumerable<AioTestCase>> GetTestCasesAsync(string jiraProjectId, int startAt = 0, int maxResults = 0, bool needDataInRTF = false);

        public Task<bool> UpdateTestResultAsync(string testCaseKey, AioTestResult result);
    }
}
