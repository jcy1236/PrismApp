using PrismApp.Modules.CaseRunner.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Interfaces
{
    public interface ITestCaseManager
    {
        // Test Case Management
        Task<AioTestCase?> GetTestCaseDetailAsync(string jiraProjectId, string testCaseId, bool needDataInRTF = false, bool needAttachments = false, int version = 1);

        Task<IEnumerable<AioTestCase>> GetTestCasesAsync(string jiraProjectId, int startAt = 0, int maxResults = 0, bool needDataInRTF = false);

        Task<string?> CreateTestCaseAsync(string jiraProjectId, object requestBody, bool needDataInRTF = false, bool uniqueAutokey = false);

        // Test Case Attachments
        Task<bool> UploadAttachmentToTestCaseAsync(string jiraProjectId, string testCaseId, string filePath, int version = 1);
        
        Task<bool> UploadAttachmentToTestCaseRTFFieldAsync(string jiraProjectId, string testCaseId, string fieldId, string filePath, int version = 1);
        
        Task<List<AttachmentDto>?> GetTestCaseAttachmentsAsync(string jiraProjectId, string testCaseId, int version = 1);
        
        Task<bool> DeleteTestCaseAttachmentAsync(string jiraProjectId, string testCaseId, string attachmentId, int version = 1);

        // Test Cycle Management
        Task<string?> CreateTestCycleAsync(string jiraProjectId, AioTestCycle testCycle);
        
        Task<AioTestCycle?> GetTestCycleDetailAsync(string jiraProjectId, string testCycleId);
        
        Task<AioTestCycleSummary?> GetTestCycleSummaryAsync(string jiraProjectId, string testCycleId);

        // Test Run Management
        Task<AioTestRunResponse?> GetTestRunsAsync(string jiraProjectId, string testCycleId, int startAt = 0, int maxResults = 0);
        
        Task<AioTestRun?> GetLatestTestRunAsync(string jiraProjectId, string testCycleId, string testId);
        
        Task<bool> CreateOrUpdateTestRunAsync(string jiraProjectId, string testCycleId, string testId, AioTestResult result);
        
        Task<bool> UploadAttachmentToTestRunAsync(string jiraProjectId, string testCycleId, string testRunId, string filePath);

        // Legacy Test Result Update (for backward compatibility)
        Task<bool> UpdateTestResultAsync(string testCaseKey, AioTestResult result);

        // Project Configuration
        Task<AioProjectConfig?> GetProjectConfigAsync(string jiraProjectId);

        Task<List<AioCustomField>?> GetCustomFieldsAsync(string jiraProjectId);

        // Batch Import
        Task<AioBatchImportResult?> ImportTestResultsBatchAsync(string jiraProjectId, string testCycleId, AioBatchImportRequest request);

        // Attachment Utilities
        byte[]? DecodeAttachmentContent(AttachmentDto? attachment);
        
        string? GetAttachmentAsText(AttachmentDto? attachment, Encoding? encoding = null);
        
        MemoryStream? GetAttachmentAsStream(AttachmentDto? attachment);
    }
}
