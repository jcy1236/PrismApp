using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.IO;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class JiraClient
    {
        private readonly HttpClient _httpClient;
        public JiraClient(string base64Credentials)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Case Operations

        public async Task<string> GetTestCaseDetailAsync(string jiraProjectId, string testCaseId, bool needDataInRTF = false, bool needAttachments = false, int version = 1)
        {
            var url = $"https://jira-stms.semes.com:18080/rest/aio-tcms-api/1.0/project/{jiraProjectId}/testcase/{testCaseId}/detail"
            + $"?needDataInRTF={needDataInRTF.ToString().ToLower()}"
            + $"&needAttachments={needAttachments.ToString().ToLower()}"
            + $"&version={version}";

            try
            {
                var response = await _httpClient.GetAsync(url);

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

        public async Task<bool> UploadAttachmentToTestCaseAsync(string jiraProjectId, string testCaseId, string filePath, int version = 1)
        {
            var url = $"https://jira-stms.semes.com:18080/rest/aio-tcms-api/1.0/project/{jiraProjectId}/testcase/{testCaseId}/attachment"
            + $"?version={version}";

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
                var response = await _httpClient.PostAsync(url, form);

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

        public async Task<bool> UploadAttachmentToTestCaseRTFFieldAsync(string jiraProjectId, string testCaseId, string fieldId, string filePath, int version = 1)
        {
            var url = $"https://jira-stms.semes.com:18080/rest/aio-tcms-api/1.0/project/{jiraProjectId}/testcase/{testCaseId}/{fieldId.ToUpper()}/attachment"
            + $"?version={version}";

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
                var response = await _httpClient.PostAsync(url, form);

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

        public async Task<string?> CreateTestCaseAsync(string jiraProjectId, object requestBody, bool needDataInRTF = false, bool uniqueAutokey = false)
        {
            var url = $"https://jira-stms.semes.com:18080/rest/aio-tcms-api/1.0/project/{jiraProjectId}/testcase"
            + $"?needDataInRTF={needDataInRTF.ToString().ToLower()}"
            + $"&uniqueAutoKey={uniqueAutokey.ToString().ToLower()}";

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

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

        #endregion


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