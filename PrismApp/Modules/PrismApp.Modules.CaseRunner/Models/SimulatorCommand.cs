using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Models
{
    public class SimulatorCommand
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Action { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Priority { get; set; } = 0; // 0=Normal, 1=High, 2=Critical
        public bool RequireAcknowledgment { get; set; } = true;
        public string CorrelationId { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();

        // 편의 메서드
        public T GetParameter<T>(string key, T defaultValue = default)
        {
            if (!Parameters.TryGetValue(key, out var value))
                return defaultValue;

            try
            {
                if (value is T directValue)
                    return directValue;

                if (typeof(T) == typeof(string))
                    return (T)(object)value.ToString();

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public void SetParameter<T>(string key, T value)
        {
            Parameters[key] = value;
        }

        public bool HasParameter(string key)
        {
            return Parameters.ContainsKey(key);
        }
    }
}
