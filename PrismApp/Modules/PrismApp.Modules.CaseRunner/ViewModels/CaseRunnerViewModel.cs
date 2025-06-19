using Prism.Commands;
using PrismApp.Core.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using PrismApp.Modules.CaseRunner.Models;
using System.Windows;

namespace PrismApp.Modules.CaseRunner.ViewModels
{
    public class CaseRunnerViewModel : RegionViewModelBase
    {
        private readonly ICaseRunnerService _caseRunnerService;

        private string _message = "CaseRunner 테스트 기능";
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand Feature1Command { get; }
        public ICommand Feature2Command { get; }
        public ICommand Feature3Command { get; }
        public ICommand Feature4Command { get; }
        public ICommand Feature5Command { get; }

        public CaseRunnerViewModel(IRegionManager regionManager, ICaseRunnerService caseRunnerService) : base(regionManager)
        {
            _caseRunnerService = caseRunnerService;

            Feature1Command = new DelegateCommand(OnFeature1);
            Feature2Command = new DelegateCommand(OnFeature2);
            Feature3Command = new DelegateCommand(OnFeature3);
            Feature4Command = new DelegateCommand(OnFeature4);
            Feature5Command = new DelegateCommand(OnFeature5);
        }

        private void OnFeature1()
        {
            Message = "Feature1 실행";
            var testCase = _caseRunnerService.Feature1(); // CaseRunnerService의 Feature1 메서드 호출
            if (testCase != null)
            {
                //MessageBox.Show(testCase.JiraProjectID.ToString());
                //Message = $"TestCase ID: {testCase.Id}, Title: {testCase.Result}"; // TestCase의 ID와 Title을 메시지로 표시
            }
            else
            {
                Message = "TestCase를 가져오지 못했습니다.";
            }
        }

        private void OnFeature2()
        {
            Message = "Feature2 실행";
            _caseRunnerService.Feature2(); // CaseRunnerService의 Feature2 메서드 호출
        }

        private void OnFeature3()
        {
            Message = "Feature3 실행";
            _caseRunnerService.Feature3(); // CaseRunnerService의 Feature3 메서드 호출
        }

        private void OnFeature4()
        {
            Message = "Feature4 실행"; 
            _caseRunnerService.Feature4(); // CaseRunnerService의 Feature4 메서드 호출
        }

        private void OnFeature5()
        {
            Message = "Feature5 실행";
            _caseRunnerService.Feature5(); // CaseRunnerService의 Feature5 메서드 호출
        }
    }
}
