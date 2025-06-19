using PrismApp.Modules.CaseRunner.Models;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner
{
    public interface ICaseRunnerService
    {
        string dummy1();

        void dummy2(string caseId);

        public Task<AioTestCase> Feature1();

        public void Feature2();

        public void Feature3();

        public void Feature4();

        public void Feature5();
    }
}
