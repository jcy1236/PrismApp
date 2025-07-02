using PrismApp.Modules.CaseRunner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner
{
    public interface ICaseRunnerService
    {
        public Task<AioTestCase> Feature1(string testCaseId);

        public void Feature2(string testCaeId);

        public void Feature3(string testCaeId);

        public void Feature4(AioTestCase newCase);

        public Task<IEnumerable<AioTestCase>> Feature5Async();
    }
}
