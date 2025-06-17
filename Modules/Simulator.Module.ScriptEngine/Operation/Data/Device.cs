using PrismApp.Modules.ScriptEngine.Definitions;
using PrismApp.Modules.WMXLoader;
using System.Collections.Concurrent;

namespace PrismApp.Modules.ScriptEngine.Operation.Data
{
    public class Device
    {
        private ConcurrentDictionary<string, ScriptComponent> _scriptCandidates = new();
        private ScriptComponent? activeScript;
        private readonly object _lock = new();
        public bool IsInit = false;
        public string? activeScriptName;

        public void RegisterScript(string scriptName, ScriptComponent script)
        {
            _scriptCandidates[scriptName] = script;
        }

        public void ResetActiveScript()
        {
            if ( activeScript != null)
            {
                lock (_lock)
                {
                    var resetScript = Activator.CreateInstance(activeScript.GetType()) as ScriptComponent;
                    activeScript = resetScript;
                    _scriptCandidates[activeScriptName] = activeScript;
                }
            }
        }

        public void RemoveScript(string scriptName)
        {
            if ( _scriptCandidates.ContainsKey(scriptName) )
            {
                _scriptCandidates.TryRemove(scriptName, out _);
                if ( activeScript == _scriptCandidates[scriptName])
                {
                    activeScript = null;
                }
            }
        }

        public void SelectScriptForExecution(string scriptName)
        {
            lock (_lock)
            {
                _scriptCandidates.TryGetValue(scriptName, out activeScript);
                activeScriptName = scriptName;
            }
        }

        public void Init()
        {
            activeScript?.Init();
        }

        public void Update()
        {
            activeScript?.Update();
        }

        public void InjectWMX(WMXApiClient client)
        {
            if ( activeScript != null)
            {
                activeScript.Wmx3Lib = client.Wmx3Lib;
                activeScript.wmxlib_cm = client.EcLib;
            }
        }
    }
}
