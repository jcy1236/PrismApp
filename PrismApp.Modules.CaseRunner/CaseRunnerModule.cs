using Microsoft.Xaml.Behaviors.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismApp.Modules.CaseRunner.Definitions;
using PrismApp.Modules.CaseRunner.Views;
using PrismApp.Modules.CaseRunner.Models;
using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner
{
    public class CaseRunnerModule : IModule
    {
        private JiraClient jira = null;
        private JiraProject jiraProject = null;
        private string jiraProjectId = string.Empty;
        private string testCaseId = string.Empty;
        private string plainText = string.Empty;
        private string base64Credentials = string.Empty;

        private TestCaseDTO newCase = null;

        public CaseRunnerModule()
        {
            jiraProject = new JiraProject
            {
                Id = "RDSIM",
                Key = "PROJ",
                Name = "Sample Project",
                Description = "This is a sample project description.",
                Lead = "John Doe",
                Url = "https://example.com/jira/projects/PROJ"
            };

            jiraProjectId = "RDSIM";
            testCaseId = "RDSIM-TC-88";

            plainText = "cy1236.jeon:4eJKRAtz&V";
            base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));

            jira = new JiraClient(base64Credentials);

            newCase = new TestCaseDTO
            {
                Title = "New Test Case",
                Description = "This is a new test case.",
                Precondition = "Precondition for the test case.",
                Type = new TestCaseDTO.TypeData
                {
                    Name = "Functional"
                },
                Tags = new List<TestCaseDTO.TagData>
                {
                new TestCaseDTO.TagData { Tag = new TestCaseDTO.TagData.TagDetail { ID = 720, Name = "a" } },
                new TestCaseDTO.TagData { Tag = new TestCaseDTO.TagData.TagDetail { ID = 721, Name = "b" } }
                },
                ScriptType = new TestCaseDTO.ScriptTypeData
                {
                    ID = 341,
                    Name = "Classic",
                    Description = "Steps represented in default representation format.",
                    IsEnabled = true
                },
                Steps = new List<TestCaseDTO.TestStep>
                {
                    new TestCaseDTO.TestStep { ID = 110335, Step = "로그인", ExpectedResult = "성공", StepType = "TEXT" },
                    new TestCaseDTO.TestStep { ID = 110336, Step = "클릭", ExpectedResult = "성공", StepType = "TEXT" }
                },
                ID = 1
            };
        }

        // Example usage of the JiraClient methods:
        public async void Feature1()
        {
            // 1) GetTestCaseDetailAsync
            var detail = await jira.GetTestCaseDetailAsync(jiraProjectId, testCaseId);

            if (detail != null)
            {
                Console.WriteLine($"=== Test Case Detail ===");
                Console.WriteLine(detail);
            }
        }

        // 2) UploadAttachmentToTestCaseAsync
        public async void Feature2()
        {
            var success = await jira.UploadAttachmentToTestCaseAsync(jiraProjectId, testCaseId, @"C:\Users\jcy1236\AppData\Local\CMakeTools\log.txt");
            Console.WriteLine(success ? "Attachment uploaded successfully." : "Failed to upload attachment.");
        }

        // 3) UploadAttachmentToTestCaseRTFFieldAsync
        public async void Feature3()
        {
            var success = await jira.UploadAttachmentToTestCaseRTFFieldAsync(jiraProjectId, testCaseId,"DESCRIPTION", @"C:\Users\jcy1236\AppData\Local\CMakeTools\log.txt");
            Console.WriteLine(success ? "RTF field attachment uploaded successfully." : "Failed to upload RTF field attachment.");
        }


        // 4) CreateTestCaseAsync
        public async void Feature4()
        {
            var result = await jira.CreateTestCaseAsync(jiraProjectId, newCase);
            Console.WriteLine(result ?? "Failed to create test case.");
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}