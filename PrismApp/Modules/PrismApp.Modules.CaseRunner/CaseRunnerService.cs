using Prism.Regions;
using PrismApp.Modules.CaseRunner.Definitions;
using PrismApp.Modules.CaseRunner.Interfaces;
using PrismApp.Modules.CaseRunner.Models;
using PrismApp.Modules.CaseRunner.Parsers;
using PrismApp.Modules.CaseRunner.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner
{
    public class CaseRunnerService : ICaseRunnerService
    {
        private readonly IRegionManager _regionManager;

        public string Message { get; set; } = string.Empty;

        public AioTestCase CurrentTestCase { get; private set; } = null;

        private ITestCaseManager jira = null;
        //private AioProjectInfo jiraProject = null;
        private string jiraProjectId = string.Empty;
        private string testCaseId = string.Empty;

        private AioTestCase newCase = null;

        public CaseRunnerService()
        {
            //jiraProject = new AioProjectInfo
            //{
            //    Id = "RES",
            //    Key = "PROJ",
            //    Name = "Sample Project",
            //    Description = "This is a sample project description."
            //};

            jiraProjectId = "RES";
            testCaseId = "RES-TC-1";

            jira = new JiraTestCaseManager();

            newCase = new AioTestCase
            {
                Title = "New Test Case",
                Description = "This is a new test case.",
                Precondition = "Precondition for the test case.",
                Type = new TypeDataDto
                {
                    Name = "Functional"
                },
                Tags = new List<TagDataDto>
                {
                    new TagDataDto { Tag = new TagDataDto.TagDetail { ID = 720, Name = "a" } },
                    new TagDataDto { Tag = new TagDataDto.TagDetail { ID = 721, Name = "b" } }
                },
                ScriptType = new AioScriptTypeDto
                {
                    ID = 341,
                    Name = "Classic",
                    Description = "Steps represented in default representation format.",
                    IsEnabled = true
                },
                Steps = new List<AioStepDto>
                {
                    new AioStepDto { ID = 110335, Step = "로그인", ExpectedResult = "성공", StepType = "TEXT" },
                    new AioStepDto { ID = 110336, Step = "클릭", ExpectedResult = "성공", StepType = "TEXT" }
                },
                ID = 1
            };
        }
        
        async Task<AioTestCase> ICaseRunnerService.Feature1()
        {
            Message = "1) GetTestCaseDetailAsync";
            var detail = await jira.GetTestCaseDetailAsync(jiraProjectId, testCaseId);
            var testCase = System.Text.Json.JsonSerializer.Deserialize<AioTestCase>(detail);
            var parser = new TestCaseParser(new List<IStepParser>());
            var parsed = parser.ParseAioTestCase(testCase);

            CurrentTestCase = parsed;

            if (detail != null)
            {
                Console.WriteLine($"=== Test Case Detail ===");
                Console.WriteLine(detail);
            }

            return testCase;
        }

        async void ICaseRunnerService.Feature2()
        {
            Message = "2) UploadAttachmentToTestCaseAsync";
            var success = await jira.UploadAttachmentToTestCaseAsync(jiraProjectId, testCaseId, @"C:\Users\jcy1236\AppData\Local\CMakeTools\log.txt");
            Console.WriteLine(success ? "Attachment uploaded successfully." : "Failed to upload attachment.");
        }

        async void ICaseRunnerService.Feature3()
        {
            Message = "3) UploadAttachmentToTestCaseRTFFieldAsync";
            var success = await jira.UploadAttachmentToTestCaseRTFFieldAsync(jiraProjectId, testCaseId, "DESCRIPTION", @"C:\Users\jcy1236\AppData\Local\CMakeTools\log.txt");
            Console.WriteLine(success ? "RTF field attachment uploaded successfully." : "Failed to upload RTF field attachment.");
        }
        
        async void ICaseRunnerService.Feature4()
        {
            Message = "4) CreateTestCaseAsync";
            var result = await jira.CreateTestCaseAsync(jiraProjectId, newCase);
            Console.WriteLine(result ?? "Failed to create test case.");
        }

        async void ICaseRunnerService.Feature5()
        {
            Message = "5) GetTestCasesAsync";
            var result = await jira.GetTestCasesAsync(jiraProjectId);
            foreach ( var res in result )
            {
                Console.WriteLine(res);
            }
        }

        string ICaseRunnerService.dummy1()
        {
            return "CaseRunner is idle.";
        }

        void ICaseRunnerService.dummy2(string caseId)
        {
            // 여기에 테스트 케이스 실행 로직을 구현합니다.
        }
    }
}
