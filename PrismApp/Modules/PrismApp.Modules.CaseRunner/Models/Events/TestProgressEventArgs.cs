using System;

namespace PrismApp.Modules.CaseRunner.Models.Events
{
    public class TestProgressEventArgs : EventArgs
    {
        public string TestCaseKey { get; set; }
        public string Message { get; set; }
        public int ProgressPercentage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public TestProgressEventArgs(string testCaseKey, string message, int progressPercentage)
        {
            TestCaseKey = testCaseKey;
            Message = message;
            ProgressPercentage = progressPercentage;
        }
    }
}
