using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrismApp.Modules.CaseRunner.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class JiraTestCaseManager : ITestCaseManager
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _config;

        private readonly ILogger<JiraTestCaseManager> _logger;

        private string plainText = string.Empty;
        private string base64Credentials = string.Empty;

        public JiraTestCaseManager()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://tcms.aiojiraapps.com/aio-tcms/api/v1/")
            };
            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            var baseUrl = _config["JiraApi:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.LogError("JiraApi:BaseUrl is not configured.");
                return;
            }
            _httpClient.BaseAddress = new Uri(baseUrl);

            var authType = _config["JiraApi:AuthType"];
            if (authType == "Basic")
            {
                var username = _config["JiraApi:BasicAuth:Username"];
                var password = _config["JiraApi:BasicAuth:Password"];
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    _logger.LogError("JiraApi:BasicAuth:Username or Password is not configured.");
                    return;
                }
                var plainText = $"{username}:{password}";
                var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            }
            else if (authType == "AioAuth")
            {
                var authToken = _config["JiraApi:AioAuth:Token"];
                if (string.IsNullOrEmpty(authToken))
                {
                    _logger.LogError("JiraApi:AioAuth:Token is not configured.");
                    return;
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("AioAuth", authToken);
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Case Operations

        async Task<AioTestCase> ITestCaseManager.GetTestCaseDetailAsync(string jiraProjectId, string testCaseId, bool needDataInRTF = false, bool needAttachments = false, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/detail"
            + "?" + $"needDataInRTF={needDataInRTF.ToString().ToLower()}"
            + "&" + $"needAttachments={needAttachments.ToString().ToLower()}"
            + "&" + $"version={version}";

            //endPoint = "project/RES/testcase/RES-TC-1/detail?version=1";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var testCase = JsonSerializer.Deserialize<AioTestCase>(jsonString);
                    
                    // If needAttachments is false, clear any attachment content to save memory
                    if (!needAttachments && testCase?.Steps != null)
                    {
                        foreach (var step in testCase.Steps)
                        {
                            ClearAttachmentContent(step.Attachments);
                            ClearAttachmentContent(step.StepAttachments);
                            ClearAttachmentContent(step.DataAttachments);
                            ClearAttachmentContent(step.ExpectedResultAttachments);
                        }
                    }
                    
                    return testCase;
                }
                Console.WriteLine($"오류 상태코드: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }
            return null;
        }

        private void ClearAttachmentContent(List<AttachmentDto> attachments)
        {
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    attachment.Content = null;
                }
            }
        }

        public async Task<bool> UploadAttachmentToTestCaseAsync(string jiraProjectId, string testCaseId, string filePath, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/attachment"
            + "?" + $"version={version}";

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("첨부할 파일이 존재하지 않습니다: {FilePath}", filePath);
                return false;
            }

            using var form = new MultipartFormDataContent();
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            form.Add(fileContent, "file", Path.GetFileName(filePath));

            try
            {
                var response = await _httpClient.PostAsync(endPoint, form);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("파일 업로드 성공");
                    return true;
                }
                _logger.LogError("첨부파일 업로드 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "첨부파일 업로드 중 예외 발생: {Message}", ex.Message);
            }
            return false;
        }

        public async Task<bool> UploadAttachmentToTestCaseRTFFieldAsync(string jiraProjectId, string testCaseId, string fieldId, string filePath, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/{fieldId.ToUpper()}/attachment"
            + "?" + $"version={version}";

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("첨부할 파일이 존재하지 않습니다: {FilePath}", filePath);
                return false;
            }

            using var form = new MultipartFormDataContent();
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            form.Add(fileContent, "file", Path.GetFileName(filePath));

            try
            {
                var response = await _httpClient.PostAsync(endPoint, form);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("RTF 필드에 첨부파일 업로드 성공");
                    return true;
                }
                _logger.LogError("RTF 필드에 첨부파일 업로드 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RTF 필드에 첨부파일 업로드 중 예외 발생: {Message}", ex.Message);
            }
            return false;
        }

        public async Task<string?> CreateTestCaseAsync(string jiraProjectId, object requestBody, bool needDataInRTF = false, bool uniqueAutokey = false)
        {
            var endPoint = $"project/{jiraProjectId}/testcase"
            + "?" + $"needDataInRTF={needDataInRTF.ToString().ToLower()}"
            + "&" + $"uniqueAutoKey={uniqueAutokey.ToString().ToLower()}";

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endPoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("테스트 케이스 생성 성공");
                    return responseData;
                }
                _logger.LogError("[오류] 상태코드: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[예외 발생]: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<AioTestCase>> GetTestCasesAsync(string jiraProjectId, int startAt = 0, int maxResults = 0, bool needDataInRTF = false)
        {
            var endPoint = $"project/{jiraProjectId}/testcase";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var aioResponse = JsonSerializer.Deserialize<AioTestCaseResponse>(json);

                return aioResponse?.Items?.Select(MapToTestCase) ?? Enumerable.Empty<AioTestCase>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get test cases for project {JiraProjectId}", jiraProjectId);
                throw;
            }
        }

        public async Task<bool> UpdateTestResultAsync(string testCaseKey, AioTestResult result)
        {
            var payload = new
            {
                testCaseKey = testCaseKey,
                status = result.Status?.ToString().ToUpper(),
                comment = result.Comment,
                executedOn = result.ExecutedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("testexecution", content);
            return response.IsSuccessStatusCode;
        }



        #endregion

        private string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".txt" => "text/plain",
                ".json" => "application/json",
                ".xml" => "application/xml",
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".zip" => "application/zip",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }

        private AioTestCase MapToTestCase(AioTestCase aioTestCase)
        {
            return new AioTestCase
            {
                Key = aioTestCase.Key,
                Title = aioTestCase.Title,
                Description = aioTestCase.Description,
                Steps = aioTestCase.Steps?.Select((step, index) => new AioStepDto
                {
                    Step = step.Step,
                    Data = step.Data,
                    ExpectedResult = step.ExpectedResult,
                    ID = index + 1
                }).ToList() ?? new List<AioStepDto>()
            };
        }

        

        


        #region Cycle Operations

        public async Task<string?> CreateTestCycleAsync(string jiraProjectId, AioTestCycle testCycle)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/detail";

            var requestBody = new
            {
                name = testCycle.Name,
                description = testCycle.Description,
                startDate = testCycle.StartDate?.ToString("yyyy-MM-dd"),
                endDate = testCycle.EndDate?.ToString("yyyy-MM-dd"),
                owner = testCycle.Owner,
                testCaseKeys = testCycle.TestCaseKeys
            };

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endPoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("테스트 사이클 생성 성공");
                    return responseData;
                }
                _logger.LogError("테스트 사이클 생성 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 사이클 생성 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<AioTestCycle?> GetTestCycleDetailAsync(string jiraProjectId, string testCycleId)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/detail";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var testCycle = JsonSerializer.Deserialize<AioTestCycle>(jsonString);
                    return testCycle;
                }
                _logger.LogError("테스트 사이클 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 사이클 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<AioTestCycleSummary?> GetTestCycleSummaryAsync(string jiraProjectId, string testCycleId)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/summary";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var summary = JsonSerializer.Deserialize<AioTestCycleSummary>(jsonString);
                    return summary;
                }
                _logger.LogError("테스트 사이클 요약 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 사이클 요약 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        #endregion


        #region Execution Management

        public async Task<AioTestRunResponse?> GetTestRunsAsync(string jiraProjectId, string testCycleId, int startAt = 0, int maxResults = 0)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/testrun";
            if (startAt > 0 || maxResults > 0)
            {
                endPoint += $"?startAt={startAt}&maxResults={maxResults}";
            }

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var testRuns = JsonSerializer.Deserialize<AioTestRunResponse>(jsonString);
                    return testRuns;
                }
                _logger.LogError("테스트 실행 목록 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 실행 목록 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<AioTestRun?> GetLatestTestRunAsync(string jiraProjectId, string testCycleId, string testId)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/testcase/{testId}/testrun";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var testRun = JsonSerializer.Deserialize<AioTestRun>(jsonString);
                    return testRun;
                }
                _logger.LogError("최신 테스트 실행 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "최신 테스트 실행 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<bool> CreateOrUpdateTestRunAsync(string jiraProjectId, string testCycleId, string testId, AioTestResult result)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/testcase/{testId}/testrun";

            var requestBody = new
            {
                status = result.Status?.ToUpper(),
                comment = result.Comment,
                executedOn = result.ExecutedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                executedBy = result.ExecutedBy,
                executionTime = result.ExecutionTime?.TotalMilliseconds,
                environment = result.Environment
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endPoint, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("테스트 실행 결과 업데이트 성공");
                    return true;
                }
                _logger.LogError("테스트 실행 결과 업데이트 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 실행 결과 업데이트 중 예외 발생: {Message}", ex.Message);
            }
            return false;
        }

        public async Task<bool> UploadAttachmentToTestRunAsync(string jiraProjectId, string testCycleId, string testRunId, string filePath)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/testrun/{testRunId}/attachment";

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("첨부할 파일이 존재하지 않습니다: {FilePath}", filePath);
                return false;
            }

            using var form = new MultipartFormDataContent();
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            form.Add(fileContent, "file", Path.GetFileName(filePath));

            try
            {
                var response = await _httpClient.PostAsync(endPoint, form);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("테스트 실행에 첨부파일 업로드 성공");
                    return true;
                }
                _logger.LogError("테스트 실행에 첨부파일 업로드 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 실행에 첨부파일 업로드 중 예외 발생: {Message}", ex.Message);
            }
            return false;
        }

        #endregion


        #region Case Listing



        #endregion


        #region Configuration

        // Project Configuration
        public async Task<AioProjectConfig?> GetProjectConfigAsync(string jiraProjectId)
        {
            var endPoint = $"project/{jiraProjectId}/config";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var config = JsonSerializer.Deserialize<AioProjectConfig>(jsonString);
                    return config;
                }
                _logger.LogError("프로젝트 설정 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "프로젝트 설정 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<List<AioCustomField>?> GetCustomFieldsAsync(string jiraProjectId)
        {
            var endPoint = $"project/{jiraProjectId}/config/customfield";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var customFields = JsonSerializer.Deserialize<List<AioCustomField>>(jsonString);
                    return customFields;
                }
                _logger.LogError("커스텀 필드 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "커스텀 필드 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        #endregion


        #region Cycle Listing

        #endregion


        #region Set Listing

        #endregion


        #region Set Operations

        #endregion


        #region Traceability

        #endregion


        #region Migration

        #endregion


        #region Attachment

        public byte[]? DecodeAttachmentContent(AttachmentDto? attachment)
        {
            if (attachment == null || !attachment.IsContentLoaded || attachment.Content == null)
                return null;

            try
            {
                return Convert.FromBase64String(attachment.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "첨부파일 디코딩 중 오류 발생: {Message}", ex.Message);
                return null;
            }
        }

        public string? GetAttachmentAsText(AttachmentDto? attachment, Encoding? encoding = null)
        {
            var bytes = DecodeAttachmentContent(attachment);
            if (bytes == null)
                return null;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(bytes);
        }

        public MemoryStream? GetAttachmentAsStream(AttachmentDto? attachment)
        {
            var bytes = DecodeAttachmentContent(attachment);
            if (bytes == null)
                return null;

            return new MemoryStream(bytes);
        }

        public async Task<List<AttachmentDto>?> GetTestCaseAttachmentsAsync(string jiraProjectId, string testCaseId, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/attachment?version={version}";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var attachments = JsonSerializer.Deserialize<List<AttachmentDto>>(jsonString);
                    _logger.LogInformation("테스트 케이스 첨부파일 목록 조회 성공");
                    return attachments;
                }
                _logger.LogError("테스트 케이스 첨부파일 목록 조회 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 케이스 첨부파일 목록 조회 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        public async Task<bool> DeleteTestCaseAttachmentAsync(string jiraProjectId, string testCaseId, string attachmentId, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/attachment/{attachmentId}?version={version}";

            try
            {
                var response = await _httpClient.DeleteAsync(endPoint);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("테스트 케이스 첨부파일 삭제 성공");
                    return true;
                }
                _logger.LogError("테스트 케이스 첨부파일 삭제 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "테스트 케이스 첨부파일 삭제 중 예외 발생: {Message}", ex.Message);
            }
            return false;
        }

        #endregion


        #region Batch

        // Batch Import
        public async Task<AioBatchImportResult?> ImportTestResultsBatchAsync(string jiraProjectId, string testCycleId, AioBatchImportRequest request)
        {
            var endPoint = $"project/{jiraProjectId}/testcycle/{testCycleId}/import/results/batch";

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endPoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<AioBatchImportResult>(jsonString);
                    _logger.LogInformation("배치 임포트 성공");
                    return result;
                }
                _logger.LogError("배치 임포트 실패: {StatusCode} {ReasonPhrase}", (int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "배치 임포트 중 예외 발생: {Message}", ex.Message);
            }
            return null;
        }

        #endregion



    }
}