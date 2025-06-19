using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismApp.Core;
using PrismApp.Modules.ScriptEngine.Operation;
using PrismApp.Modules.BaseApp.Views;

namespace PrismApp.Modules.BaseApp
{
    public class BaseApp : IModule
    {
        private readonly IRegionManager _regionManager;

        public BaseApp(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "CaseRunnerView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterSingleton<OperationManager>();
        }
    }
}