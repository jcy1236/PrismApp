using Prism.Ioc;
using Prism.Modularity;
using PrismApp.Modules.VxStudio;
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
            moduleCatalog.AddModule<VxStudio>();

        }
    }
}
