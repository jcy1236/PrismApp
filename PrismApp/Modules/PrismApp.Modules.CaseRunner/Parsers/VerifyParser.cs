using PrismApp.Modules.CaseRunner.Models;
using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Parsers
{
    public class VerifyParser : IStepParser
    {
        public string ActionType => "VERIFY";

        public SimulatorCommand CreateCommand(AioTestStep step)
        {
            return new SimulatorCommand
            {
                Action = "Verify",
                Parameters = new Dictionary<string, object>
                {
                    ["target"] = step.Parameters.GetValueOrDefault("target", "status"),
                    ["expectedValue"] = step.ExpectedResult,
                    ["tolerance"] = step.Parameters.GetValueOrDefault("tolerance", 0.1),
                    ["retryCount"] = step.Parameters.GetValueOrDefault("retryCount", 3)
                },
                Timeout = step.Timeout ?? TimeSpan.FromSeconds(15)
            };
        }
    }
}
