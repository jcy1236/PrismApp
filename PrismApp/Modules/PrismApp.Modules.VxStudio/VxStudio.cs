using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismApp.Core;
using PrismApp.Modules.ScriptEngine.Operation;
using PrismApp.Modules.VxStudio.Views;

namespace PrismApp.Modules.VxStudio
{
    public class VxStudio : IModule
    {
        private readonly IRegionManager _regionManager;

        public VxStudio(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "ViewA");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterSingleton<OperationManager>();
        }
    }
}