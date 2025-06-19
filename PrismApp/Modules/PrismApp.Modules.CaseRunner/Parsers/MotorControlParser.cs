using PrismApp.Modules.CaseRunner.Models;
using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Parsers
{
    public class MotorControlParser : IStepParser
    {
        public string ActionType => "MOTOR_CONTROL";

        public SimulatorCommand CreateCommand(AioTestStep step)
        {
            return new SimulatorCommand
            {
                Action = "ControlMotor",
                Parameters = new Dictionary<string, object>
                {
                    ["motorId"] = step.Parameters.GetValueOrDefault("motorId", "motor1"),
                    ["speed"] = step.Parameters.GetValueOrDefault("speed", 0),
                    ["direction"] = step.Parameters.GetValueOrDefault("direction", "forward"),
                    ["duration"] = step.Parameters.GetValueOrDefault("duration", 5)
                },
                Timeout = step.Timeout ?? TimeSpan.FromSeconds(30)
            };
        }
    }
}
