using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Models.Commands
{
    public static class CommandBuilders
    {
        public static SimulatorCommandBuilder SetParameter(string parameterName, object value)
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.SetParameter)
                .WithParameter("parameter", parameterName)
                .WithParameter("value", value);
        }

        public static SimulatorCommandBuilder ReadSensor(string sensorId)
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.ReadSensor)
                .WithParameter("sensor_id", sensorId);
        }

        public static SimulatorCommandBuilder Wait(double seconds)
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.Wait)
                .WithParameter("duration", seconds)
                .WithTimeout(TimeSpan.FromSeconds(seconds + 5));
        }

        public static SimulatorCommandBuilder WaitForCondition(string condition, double timeoutSeconds = 30)
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.WaitForCondition)
                .WithParameter("condition", condition)
                .WithTimeout(TimeSpan.FromSeconds(timeoutSeconds));
        }

        public static SimulatorCommandBuilder MoveActuator(string actuatorId, double position)
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.MoveActuator)
                .WithParameter("actuator_id", actuatorId)
                .WithParameter("position", position);
        }

        public static SimulatorCommandBuilder SendMessage(string target, string message, object data = null)
        {
            var builder = SimulatorCommandBuilder.Create(SimulatorActions.SendMessage)
                .WithParameter("target", target)
                .WithParameter("message", message);

            if (data != null)
                builder.WithParameter("data", data);

            return builder;
        }

        public static SimulatorCommandBuilder LoadScenario(string scenarioPath)
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.LoadScenario)
                .WithParameter("scenario_path", scenarioPath);
        }

        public static SimulatorCommandBuilder GetStatus()
        {
            return SimulatorCommandBuilder.Create(SimulatorActions.GetStatus)
                .RequireAck(false);
        }

        public static SimulatorCommandBuilder Initialize(Dictionary<string, object> config = null)
        {
            var builder = SimulatorCommandBuilder.Create(SimulatorActions.Initialize)
                .WithTimeout(TimeSpan.FromMinutes(2));

            if (config != null)
                builder.WithParameters(config);

            return builder;
        }
    }
}
