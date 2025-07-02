using Prism.Commands;
using PrismApp.Core.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using PrismApp.Modules.CaseRunner.Models;
using System.Windows;
using System.Collections.ObjectModel;

namespace PrismApp.Modules.CaseRunner.ViewModels
{
    public class CaseRunnerViewModel : RegionViewModelBase
    {
        private readonly ICaseRunnerService _caseRunnerService;

        private string _message;
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

        public ObservableCollection<AioTestCase> AioTestCases { get; set; }

        private AioTestCase _selectedTestCase;
        public AioTestCase SelectedTestCase
        {
            get => _selectedTestCase;
            set => SetProperty(ref _selectedTestCase, value);
        }

        public CaseRunnerViewModel(IRegionManager regionManager, ICaseRunnerService caseRunnerService) : base(regionManager)
        {
            _caseRunnerService = caseRunnerService;

            Feature1Command = new DelegateCommand(OnFeature1);
            Feature2Command = new DelegateCommand(OnFeature2);
            Feature3Command = new DelegateCommand(OnFeature3);
            Feature4Command = new DelegateCommand(OnFeature4);
            Feature5Command = new DelegateCommand(OnFeature5);

            // 임의의 데이터 추가
            AioTestCases = new();
            //AioTestCases = new ObservableCollection<AioTestCase>
            //{
            //    new AioTestCase { ID = 1, Title = "테스트 케이스 1", Description = "설명 1", Steps = new System.Collections.Generic.List<AioStepDto> { new AioStepDto { Step = "Step1", ExpectedResult = "Result1", StepType = "Type1" } } },
            //    new AioTestCase { ID = 2, Title = "테스트 케이스 2", Description = "설명 2", Steps = new System.Collections.Generic.List<AioStepDto> { new AioStepDto { Step = "Step2", ExpectedResult = "Result2", StepType = "Type2" } } },
            //    new AioTestCase { ID = 3, Title = "테스트 케이스 3", Description = "설명 3", Steps = new System.Collections.Generic.List<AioStepDto> { new AioStepDto { Step = "Step3", ExpectedResult = "Result3", StepType = "Type3" } } }
            //};
        }

        private void OnFeature1()
        {
            Message = "Feature1 실행";

            var testCase = _caseRunnerService.Feature1(SelectedTestCase.Key); // CaseRunnerService의 Feature1 메서드 호출
            if (testCase != null)
            {
                //MessageBox.Show(testCase.JiraProjectID.ToString());
                Message = $"TestCase ID: {testCase.Id}, Title: {testCase.Result}"; // TestCase의 ID와 Title을 메시지로 표시
            }
            else
            {
                Message = "TestCase를 가져오지 못했습니다.";
            }
        }

        private void OnFeature2()
        {
            Message = "Feature2 실행";
            _caseRunnerService.Feature2(SelectedTestCase.Key); // CaseRunnerService의 Feature2 메서드 호출
        }

        private void OnFeature3()
        {
            Message = "Feature3 실행";
            _caseRunnerService.Feature3(SelectedTestCase.Key); // CaseRunnerService의 Feature3 메서드 호출
        }

        private void OnFeature4()
        {
            Message = "Feature4 실행"; 
            _caseRunnerService.Feature4(new AioTestCase()); // CaseRunnerService의 Feature4 메서드 호출
        }

        private async void OnFeature5()
        {
            Message = "Feature5 실행";
            var cases = await _caseRunnerService.Feature5Async();

            AioTestCases.Clear();
            foreach (var c in cases)
            {
                var caseDetail = await _caseRunnerService.Feature1(c.Key);

                foreach ( var step in caseDetail.Steps )
                {
                    c.Steps.Add(step);
                }
                AioTestCases.Add(c);
            }
        }
    }
}
