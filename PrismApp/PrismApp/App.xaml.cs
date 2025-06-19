using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using PrismApp.Modules.BaseApp;
using PrismApp.Modules.WMXLoader;
using PrismApp.Modules.CaseRunner;
using PrismApp.Services;
using PrismApp.Services.Interfaces;
using PrismApp.Views;
using System.Windows;

namespace PrismApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<BaseApp>();
            //moduleCatalog.AddModule<WMXLoaderModule>(); // WMXLoader 모듈 추가
            moduleCatalog.AddModule<CaseRunnerModule>(); // CaseRunner 모듈 추가
        }
    }
}
