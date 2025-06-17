using PrismApp.Modules.WMXLoader.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;
using System.Diagnostics;

namespace PrismApp.Modules.WMXLoader
{
    public class WMXLoaderModule: IModule
    {
        private string WMX3Path = @"C:\Program Files\SoftServo\WMX3\Lib";
        
        private Dictionary<string, Assembly> m_wmx3Assembly = new();

        private Dictionary<string, MethodInfo> m_wmx3Method = new();

        private List<string> _wmx3dlls = new([
            "WMX3Api_CLRLib.dll",
            "AdvancedMotionApi_CLRLib.dll",
            "ApiBufferApi_CLRLib.dll",
            "CompensationApi_CLRLib.dll",
            "EcApi_CLRLib.dll",
            "IOApi_CLRLib.dll",
            "SimuApi_CLRLib.dll"
            ]);


        #region WMX3 Class Types

        public dynamic Wmx3Lib { get; private set; } = null;

        public dynamic EcLib { get; private set; } = null;

        public dynamic Wmx3Lib_Io { get; private set; } = null;

        public dynamic Wmx3Lib_Cm { get; private set; } = null;

        public dynamic wmxStatus { get; private set; } = null;

        public dynamic Wmx3Sim { get; private set; } = null;

        public dynamic SimInfo { get; private set; } = null;

        #endregion

        private Type typeWMX3Api;

        private Type typeEcat;

        private Type typeIo;

        private Type typeEngineStatus;

        private Type typeSimu;

        private Type typeSimuMasterInfo;


        public void OnInitialized(IContainerProvider containerProvider)
        {
            Debug.WriteLine("WMXLoaderModule Initialized");

            LoadAssembly();

            CreateInstance();

            CreateMethodInfo();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IWMXService, WMXService>();
        }

        private void LoadAssembly()
        {
            m_wmx3Assembly.Clear();

            foreach ( string dllName in _wmx3dlls )
            {
                string dllPath = Path.Combine(WMX3Path, dllName);
                Assembly assembly = Assembly.LoadFrom(dllPath);
                m_wmx3Assembly.Add(dllName, assembly);
            }
        }

        private void CreateInstance()
        {
            try
            {
                typeWMX3Api = m_wmx3Assembly["WMX3Api_CLRLib.dll"].GetType("WMX3ApiCLR.WMX3Api");
                Wmx3Lib = Activator.CreateInstance(typeWMX3Api);

                typeEcat = m_wmx3Assembly["EcApi_CLRLib.dll"].GetType("WMX3ApiCLR.EcApiCLR.Ecat");
                EcLib = Activator.CreateInstance(typeEcat, Wmx3Lib);

                typeIo = m_wmx3Assembly["IOApi_CLRLib.dll"].GetType("WMX3ApiCLR.Io");
                Wmx3Lib_Io = Activator.CreateInstance(typeIo, Wmx3Lib);

                typeEngineStatus = m_wmx3Assembly["WMX3Api_CLRLib.dll"].GetType("WMX3ApiCLR.EngineStatus");
                wmxStatus = Activator.CreateInstance(typeEngineStatus);

                typeSimu = m_wmx3Assembly["SimuApi_CLRLib.dll"].GetType("WMX3ApiCLR.SimuApiCLR.Simu");
                Wmx3Sim = Activator.CreateInstance(typeSimu, Wmx3Lib);

                typeSimuMasterInfo = m_wmx3Assembly["SimuApi_CLRLib.dll"].GetType("WMX3ApiCLR.SimuApiCLR.SimuMasterInfo");
                SimInfo = Activator.CreateInstance(typeSimuMasterInfo);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private void CreateMethodInfo()
        {
            try
            {
                m_wmx3Method["SetInBit"] = typeSimu.GetMethod("SetInBit");
                m_wmx3Method["GetOutBitEx"] = typeIo.GetMethod("GetOutBitEx");
                m_wmx3Method["StopCommunication"] = typeWMX3Api.GetMethod("StopCommunication", Array.Empty<Type>());
                m_wmx3Method["GetEngineStatus"] = typeWMX3Api.GetMethod("GetEngineStatus");
                m_wmx3Method["GetInBitEx"] = typeIo.GetMethod("GetInBitEx");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}