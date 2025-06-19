using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Models.Commands
{
    // 명령어 빌더 클래스들
    public class SimulatorCommandBuilder
    {
        private readonly SimulatorCommand _command;

        private SimulatorCommandBuilder(string action)
        {
            _command = new SimulatorCommand { Action = action };
        }

        public static SimulatorCommandBuilder Create(string action)
        {
            return new SimulatorCommandBuilder(action);
        }

        public SimulatorCommandBuilder WithId(string id)
        {
            _command.Id = id;
            return this;
        }

        public SimulatorCommandBuilder WithParameter<T>(string key, T value)
        {
            _command.SetParameter(key, value);
            return this;
        }

        public SimulatorCommandBuilder WithParameters(Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                _command.Parameters[param.Key] = param.Value;
            }
            return this;
        }

        public SimulatorCommandBuilder WithTimeout(TimeSpan timeout)
        {
            _command.Timeout = timeout;
            return this;
        }

        public SimulatorCommandBuilder WithTimeout(int seconds)
        {
            _command.Timeout = TimeSpan.FromSeconds(seconds);
            return this;
        }

        public SimulatorCommandBuilder WithPriority(int priority)
        {
            _command.Priority = priority;
            return this;
        }

        public SimulatorCommandBuilder WithCorrelationId(string correlationId)
        {
            _command.CorrelationId = correlationId;
            return this;
        }

        public SimulatorCommandBuilder RequireAck(bool require = true)
        {
            _command.RequireAcknowledgment = require;
            return this;
        }

        public SimulatorCommandBuilder WithMetadata(string key, string value)
        {
            _command.Metadata[key] = value;
            return this;
        }

        public SimulatorCommand Build()
        {
            return _command;
        }
    }
}
