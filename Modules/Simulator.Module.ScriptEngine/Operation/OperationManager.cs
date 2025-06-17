using PrismApp.Modules.WMXLoader;

namespace PrismApp.Modules.ScriptEngine.Operation
{
    public class OperationManager
    {
        private readonly IWMXService _wmxService;
        private OperationEngine? _operationEngine;

        public OperationManager(IWMXService wmxService)
        {
            _wmxService = wmxService;
        }

        public void Initialize()
        {
            _operationEngine = new OperationEngine(_wmxService);
        }
    }
}
