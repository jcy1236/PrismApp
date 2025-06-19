using PrismApp.Modules.CaseRunner.Models;
using PrismApp.Modules.CaseRunner.Models.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Interfaces
{
    public interface ITestExecutor
    {
        event EventHandler<TestProgressEventArgs> ProgressChanged;
        Task<AioTestResult> ExecuteTestCaseAsync(AioTestCase testCase, CancellationToken cancellationToken);
        void Stop();
    }
}
