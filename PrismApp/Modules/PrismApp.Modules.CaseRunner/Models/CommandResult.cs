using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class CommandResult
    {
        public string CommandId { get; set; }
        public bool Success { get; set; }
        public string Result { get; set; }
        public TestStatus Status { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public TimeSpan Duration { get; set; }
        public Dictionary<string, object> Data { get; set; } = new();
        public SimulatorResponseCode ResponseCode { get; set; }
        public int StepOrder { get; set; }

        // 정적 생성 메서드
        public static CommandResult SetSuccess(string commandId, string result = null, Dictionary<string, object> data = null)
        {
            return new CommandResult
            {
                CommandId = commandId,
                Success = true,
                Result = result,
                Data = data ?? new Dictionary<string, object>(),
                ResponseCode = SimulatorResponseCode.Success
            };
        }

        public static CommandResult SetFailure(string commandId, string errorMessage, Exception exception = null)
        {
            return new CommandResult
            {
                CommandId = commandId,
                Success = false,
                ErrorMessage = errorMessage,
                Exception = exception,
                ResponseCode = SimulatorResponseCode.Error
            };
        }

        public static CommandResult SetTimeout(string commandId)
        {
            return new CommandResult
            {
                CommandId = commandId,
                Success = false,
                ErrorMessage = "Command execution timed out",
                ResponseCode = SimulatorResponseCode.Timeout
            };
        }
    }
}
