using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PrismApp.Modules.CaseRunner.Extensions
{
    public static class ParameterExtensions
    {
        public static T GetValueOrDefault<T>(this Dictionary<string, object> parameters, string key, T defaultValue)
        {
            if (!parameters.TryGetValue(key, out var value))
                return defaultValue;

            try
            {
                if (value is JsonElement jsonElement)
                {
                    return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
