using PrismApp.Modules.CaseRunner.Models;
using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Parsers
{
    public class WaitParser : IStepParser
    {
        public string ActionType => "WAIT";

        public SimulatorCommand CreateCommand(AioTestStep step)
        {
            var duration = step.Parameters.GetValueOrDefault("duration", 1.0);

            return new SimulatorCommand
            {
                Action = "Wait",
                Parameters = new Dictionary<string, object>
                {
                    ["duration"] = duration
                },
                Timeout = TimeSpan.FromSeconds((double)duration + 5)
            };
        }
    }
}
