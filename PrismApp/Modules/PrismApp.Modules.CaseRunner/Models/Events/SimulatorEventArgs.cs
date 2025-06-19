using System;

namespace PrismApp.Modules.CaseRunner.Models.Events
{
    public class SimulatorEventArgs : EventArgs
    {
        public string EventType { get; set; }
        public string Source { get; set; }
        public object Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public SimulatorEventArgs(string eventType, string source, object data = null)
        {
            EventType = eventType;
            Source = source;
            Data = data;
        }
    }
}
