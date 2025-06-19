using PrismApp.Modules.CaseRunner.Models;
using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Parsers
{
    public class SensorReadParser : IStepParser
    {
        public string ActionType => "SENSOR_READ";

        public SimulatorCommand CreateCommand(AioTestStep step)
        {
            return new SimulatorCommand
            {
                Action = "ReadSensor",
                Parameters = new Dictionary<string, object>
                {
                    ["sensorId"] = step.Parameters.GetValueOrDefault("sensorId", "default"),
                    ["sensorType"] = step.Parameters.GetValueOrDefault("sensorType", "temperature"),
                    ["readCount"] = step.Parameters.GetValueOrDefault("readCount", 1),
                    ["interval"] = step.Parameters.GetValueOrDefault("interval", 100)
                },
                Timeout = step.Timeout ?? TimeSpan.FromSeconds(10)
            };
        }
    }
}
