using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PrismApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateToCaseRunnerCommand = new DelegateCommand(NavigateToCaseRunner);
        }

        public DelegateCommand NavigateToCaseRunnerCommand { get; }

        private void NavigateToCaseRunner()
        {
            _regionManager.RequestNavigate("MainRegion", "ViewA");
        }
    }
}
