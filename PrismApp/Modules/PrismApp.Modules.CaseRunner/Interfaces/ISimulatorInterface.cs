using PrismApp.Modules.CaseRunner.Models;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Interfaces
{
    public interface ISimulatorInterface
    {
        Task<bool> ConnectAsync();
        Task<CommandResult> SendCommandAsync(SimulatorCommand command);
        Task<SensorData> ReadSensorAsync(string sensorId);
        void Disconnect();
        bool IsConnected { get; }
    }
}
