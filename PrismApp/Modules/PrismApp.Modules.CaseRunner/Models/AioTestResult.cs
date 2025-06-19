using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class AioTestResult
    {
        public string TestCaseKey { get; set; }
        public TestStatus Status { get; set; }
        public string Comment { get; set; }
        public List<CommandResult> StepResults { get; set; } = new();
        public DateTime ExecutedAt { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
