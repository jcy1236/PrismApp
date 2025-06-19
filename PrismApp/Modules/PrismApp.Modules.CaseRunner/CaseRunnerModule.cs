using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using Prism.Regions;
using PrismApp.Modules.CaseRunner.Definitions;
using PrismApp.Modules.CaseRunner.Models;
using PrismApp.Modules.CaseRunner.Parsers;
using PrismApp.Modules.CaseRunner.Services;
using PrismApp.Modules.CaseRunner.Views;
using System;
using System.Collections.Generic;
using PrismApp.Modules.CaseRunner.ViewModels;

namespace PrismApp.Modules.CaseRunner
{
    public class CaseRunnerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CaseRunnerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // 필요 시 초기 Navigation 수행
            _regionManager.RequestNavigate("MainRegion", "CaseRunnerView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // View 등록
            containerRegistry.RegisterForNavigation<CaseRunnerView, CaseRunnerViewModel>("CaseRunnerView");

            // Parser 등록
            containerRegistry.RegisterSingleton<ITestCaseParser, TestCaseParser>();

            // Service 등록
            containerRegistry.Register<ICaseRunnerService, CaseRunnerService>();

            // Step Parsers 등록
            containerRegistry.Register<IStepParser, SensorReadParser>("SENSOR_READ");
            containerRegistry.Register<IStepParser, MotorControlParser>("MOTOR_CONTROL");
            containerRegistry.Register<IStepParser, WaitParser>("WAIT");
            containerRegistry.Register<IStepParser, VerifyParser>("VERIFY");
        }
    }
}