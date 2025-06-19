using PrismApp.Modules.CaseRunner.Models;

namespace PrismApp.Modules.CaseRunner.Parsers
{
    public interface IStepParser
    {
        string ActionType { get; }
        SimulatorCommand CreateCommand(AioTestStep step);
    }
}
