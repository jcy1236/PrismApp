using System;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Models
{
    public enum CommandStatus
    {
        Queued,
        Executing,
        Completed,
        Failed,
        Cancelled,
        Timeout
    }

    public class CommandQueueItem
    {
        public SimulatorCommand Command { get; set; }
        public DateTime QueuedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public CommandResult Result { get; set; }
        public CommandStatus Status { get; set; } = CommandStatus.Queued;
        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;
        public TaskCompletionSource<CommandResult> CompletionSource { get; set; }

        public CommandQueueItem(SimulatorCommand command)
        {
            Command = command;
            CompletionSource = new TaskCompletionSource<CommandResult>();
        }

        public TimeSpan? GetQueueTime()
        {
            return StartedAt?.Subtract(QueuedAt);
        }

        public TimeSpan? GetExecutionTime()
        {
            if (StartedAt.HasValue && CompletedAt.HasValue)
                return CompletedAt.Value.Subtract(StartedAt.Value);
            return null;
        }
    }
}
