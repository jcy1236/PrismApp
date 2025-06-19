using System;
using System.Collections.Generic;

namespace PrismApp.Modules.CaseRunner.Models
{
    public enum SensorStatus
    {
        Normal,
        Warning,
        Error,
        Offline,
        Calibrating,
        OutOfRange
    }

    public class SensorData
    {
        public string SensorId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string Unit { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public SensorStatus Status { get; set; } = SensorStatus.Normal;
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public double? Accuracy { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();

        public T GetValue<T>(T defaultValue = default)
        {
            try
            {
                if (Value is T directValue)
                    return directValue;

                return (T)Convert.ChangeType(Value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public bool IsWithinRange()
        {
            if (!MinValue.HasValue || !MaxValue.HasValue)
                return true;

            if (Value is IComparable comparable)
            {
                var numericValue = Convert.ToDouble(Value);
                return numericValue >= MinValue && numericValue <= MaxValue;
            }

            return true;
        }
    }
}
