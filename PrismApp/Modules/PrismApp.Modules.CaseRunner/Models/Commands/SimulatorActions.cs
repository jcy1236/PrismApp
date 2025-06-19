namespace PrismApp.Modules.CaseRunner.Models.Commands
{
    // 기본 명령어 타입들
    public static class SimulatorActions
    {
        // 시스템 제어
        public const string Initialize = "initialize";
        public const string Start = "start";
        public const string Stop = "stop";
        public const string Reset = "reset";
        public const string Shutdown = "shutdown";
        public const string Pause = "pause";
        public const string Resume = "resume";

        // 파라미터 제어
        public const string SetParameter = "set_parameter";
        public const string GetParameter = "get_parameter";
        public const string SetMultipleParameters = "set_multiple_parameters";

        // 센서 제어
        public const string ReadSensor = "read_sensor";
        public const string ReadAllSensors = "read_all_sensors";
        public const string SetSensorValue = "set_sensor_value";
        public const string CalibrateSensor = "calibrate_sensor";

        // 액추에이터 제어
        public const string MoveActuator = "move_actuator";
        public const string SetActuatorPosition = "set_actuator_position";
        public const string GetActuatorStatus = "get_actuator_status";

        // 시뮬레이션 제어
        public const string LoadScenario = "load_scenario";
        public const string RunScenario = "run_scenario";
        public const string StopScenario = "stop_scenario";
        public const string GetSimulationState = "get_simulation_state";

        // 진단 및 모니터링
        public const string GetStatus = "get_status";
        public const string GetDiagnostics = "get_diagnostics";
        public const string RunSelfTest = "run_self_test";
        public const string GetLogs = "get_logs";

        // 통신 제어
        public const string SendMessage = "send_message";
        public const string BroadcastMessage = "broadcast_message";
        public const string GetConnectionStatus = "get_connection_status";

        // 대기 및 동기화
        public const string Wait = "wait";
        public const string WaitForCondition = "wait_for_condition";
        public const string WaitForSensor = "wait_for_sensor";
        public const string Synchronize = "synchronize";
    }
}
