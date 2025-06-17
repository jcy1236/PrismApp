using PrismApp.Modules.WMXLoader;
using System.Timers;

namespace PrismApp.Modules.ScriptEngine.Operation
{
    public class OperationEngine
    {
        private IWMXService _wmxService;
        private WMXApiClient _wmxApiClient;

        private System.Timers.Timer tickTimer = null;

        public OperationEngine(IWMXService wmxService)
        {
            _wmxService = wmxService;
            _wmxApiClient = _wmxService.GetClient();

            tickTimer = new System.Timers.Timer(2000);
            tickTimer.AutoReset = true;
            tickTimer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            RunActiveDevices();
        }

        private void RunActiveDevices()
        {
            //var game = new ScriptProject();
        }

        public void Stop()
        {
            tickTimer.Stop();
        }
    }
}
