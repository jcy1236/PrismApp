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
            // Jira API 인증 정보 설정 (예시: Basic Auth)
#if _SEMES
            plainText = "cy1236.jeon:4eJKRAtz&V";   // 계정:cy1236.jeon, API Token:4eJKRAtz&V
            base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
#else
            string authToken = "MzQwOTc1NDMtYTYwNC0zYjNkLWFmZWMtZGMwOTBmYjJkN2JhLmNkMmEyN2JjLTc3ZjktNDM1Mi04NTI3LTY0MDk0MTUyMDBkMQ==";   // AIO Test Management API Token            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("AioAuth", authToken);
#endif
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.BaseAddress = new Uri("https://withje2.atlassian.net/rest");
            //var baseUrl = _config["Jira:BaseUrl"];
            //var token = _config["Jira:Token"];
        }

        #region Case Operations

        async Task<string> ITestCaseManager.GetTestCaseDetailAsync(string jiraProjectId, string testCaseId, bool needDataInRTF = false, bool needAttachments = false, int version = 1)
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
                    return await response.Content.ReadAsStringAsync();
                }
                Console.WriteLine($"오류 상태코드: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }
            return null;
        }

        async Task<bool> ITestCaseManager.UploadAttachmentToTestCaseAsync(string jiraProjectId, string testCaseId, string filePath, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/attachment"
            + "?" + $"version={version}";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"첨부할 파일이 존재하지 않습니다: {filePath}");
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
                    Console.WriteLine("파일 업로드 성공");
                    return true;
                }
                Console.WriteLine($"첨부파일 업로드 실패: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"첨부파일 업로드 중 예외 발생: {ex.Message}");
            }
            return false;
        }

        async Task<bool> ITestCaseManager.UploadAttachmentToTestCaseRTFFieldAsync(string jiraProjectId, string testCaseId, string fieldId, string filePath, int version = 1)
        {
            var endPoint = $"project/{jiraProjectId}/testcase/{testCaseId}/{fieldId.ToUpper()}/attachment"
            + "?" + $"version={version}";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"첨부할 파일이 존재하지 않습니다: {filePath}");
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
                    Console.WriteLine("RTF 필드에 첨부파일 업로드 성공");
                    return true;
                }
                Console.WriteLine($"RTF 필드에 첨부파일 업로드 실패: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RTF 필드에 첨부파일 업로드 중 예외 발생: {ex.Message}");
            }
            return false;
        }

        async Task<string?> ITestCaseManager.CreateTestCaseAsync(string jiraProjectId, object requestBody, bool needDataInRTF = false, bool uniqueAutokey = false)
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
                    Console.WriteLine("테스트 케이스 생성 성공");
                    return responseData;
                }
                Console.WriteLine($"[오류] 상태코드: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[예외 발생]: {ex.Message}\n{ex.StackTrace}");
            }
            return null;
        }

        async Task<IEnumerable<AioTestCase>> ITestCaseManager.GetTestCasesAsync(string jiraProjectId, int startAt = 0, int maxResults = 0, bool needDataInRTF = false)
        {
            var endPoint = $"project/{jiraProjectId}/testcase";

            try
            {
                var response = await _httpClient.GetAsync(endPoint);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var aioResponse = JsonSerializer.Deserialize<AioTestCaseResponse>(json);

                return aioResponse.TestCases.Select(MapToTestCase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //_logger.LogError(ex, "Failed to get test cases for plan {TestPlanId}", testPlanId);
                throw;
            }
        }

        async Task<bool> ITestCaseManager.UpdateTestResultAsync(string testCaseKey, AioTestResult result)
        {
            var payload = new
            {
                testCaseKey = testCaseKey,
                status = result.Status.ToString().ToUpper(),
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

        #endregion


        #region Execution Management

        #endregion


        #region Case Listing



        #endregion


        #region Batch

        #endregion


        #region Configuration

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

        #endregion
    }
}