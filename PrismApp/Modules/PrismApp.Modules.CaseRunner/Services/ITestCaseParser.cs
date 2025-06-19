using PrismApp.Modules.CaseRunner.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PrismApp.Modules.CaseRunner.Services
{
    public interface ITestCaseParser
    {
        AioTestCase ParseAioTestCase(AioTestCase aioTestCase);
        List<SimulatorCommand> ParseStepsToCommands(List<AioTestStep> testSteps);
        AioTestStep ParseStepDescription(string description, string testData, string expectedResult);

        //AioTestCase ParseFromAio(AioTestCase aioTestCase);
        //List<AioTestCase> ParseMultipleFromAio(IEnumerable<AioTestCase> aioTestCases);
        //SimulatorCommand ParseStepToCommand(TestStep step);
        //ValidationRule ParseExpectedResult(string expectedResult);
    }
}
