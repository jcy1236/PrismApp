using PrismApp.Modules.CaseRunner.Interfaces;
using PrismApp.Modules.CaseRunner.Models;
using PrismApp.Modules.CaseRunner.Models.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrismApp.Modules.CaseRunner.Services
{
    public class TestExecutor : ITestExecutor
    {
        private readonly ISimulatorInterface _simulator;
        //private readonly IGameLoopService _gameLoop;
        //private readonly ILogger<TestExecutor> _logger;

        public event EventHandler<TestProgressEventArgs> ProgressChanged;

        public TestExecutor(ISimulatorInterface simulator /*, IGameLoopService gameLoop, ILogger<TestExecutor> logger */)
        {
            _simulator = simulator;
            //_gameLoop = gameLoop;
            //_logger = logger;
        }

        public async Task<AioTestResult> ExecuteTestCaseAsync(AioTestCase testCase, CancellationToken cancellationToken)
        {
            var result = new AioTestResult
            {
                TestCaseKey = testCase.Key,
                ExecutedAt = DateTime.UtcNow
            };

            var stopwatch = Stopwatch.StartNew();

            try
            {
                if (!_simulator.IsConnected)
                {
                    await _simulator.ConnectAsync();
                }

                OnProgressChanged(new TestProgressEventArgs(testCase.Key, "Starting test execution", 0));

                for (int i = 0; i < testCase.Steps.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var step = testCase.Steps[i];
                    //var stepResult = await ExecuteStepAsync(step, cancellationToken);
                    //result.StepResults.Add(stepResult);

                    //var progress = (i + 1) * 100 / testCase.Steps.Count;
                    //OnProgressChanged(new TestProgressEventArgs(testCase.Key, $"Step {i + 1} completed", progress));

                    //if (stepResult.Status == TestStatus.Failed)
                    //{
                    //    result.Status = TestStatus.Failed;
                    //    break;
                    //}
                }

                result.Status = result.StepResults.All(s => s.Status == TestStatus.Passed)
                    ? TestStatus.Passed
                    : TestStatus.Failed;
            }
            catch (OperationCanceledException)
            {
                result.Status = TestStatus.Skipped;
                result.Comment = "Test execution was cancelled";
            }
            catch (Exception ex)
            {
                result.Status = TestStatus.Failed;
                result.Comment = ex.Message;
                //_logger.LogError(ex, "Test execution failed for {TestCaseKey}", testCase.Key);
            }
            finally
            {
                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;
            }

            return result;
        }

        private async Task<CommandResult> ExecuteStepAsync(AioTestStep step, CancellationToken cancellationToken)
        {
            var stepResult = new CommandResult { StepOrder = step.Order };
            var stepStopwatch = Stopwatch.StartNew();

            try
            {
                // 시뮬레이터 명령 실행
                var command = CreateSimulatorCommand(step);
                var commandResult = await _simulator.SendCommandAsync(command);

                if (!commandResult.Success)
                {
                    stepResult.Status = TestStatus.Failed;
                    stepResult.ErrorMessage = commandResult.ErrorMessage;
                    return stepResult;
                }

                // 결과 검증
                if (await ValidateStepResult(step, commandResult))
                {
                    stepResult.Status = TestStatus.Passed;
                    stepResult.Result = commandResult.Result;
                }
                else
                {
                    stepResult.Status = TestStatus.Failed;
                    stepResult.Result = commandResult.Result;
                    stepResult.ErrorMessage = "Expected result validation failed";
                }
            }
            catch (Exception ex)
            {
                stepResult.Status = TestStatus.Failed;
                stepResult.ErrorMessage = ex.Message;
            }
            finally
            {
                stepStopwatch.Stop();
                stepResult.Duration = stepStopwatch.Elapsed;
            }

            return stepResult;
        }

        private SimulatorCommand CreateSimulatorCommand(AioTestStep step)
        {
            // TestStep의 Action과 Parameters를 SimulatorCommand로 변환
            return new SimulatorCommand
            {
                Action = step.Action,
                Parameters = step.Parameters,
                Timeout = step.Timeout ?? TimeSpan.FromSeconds(30)
            };
        }

        private async Task<bool> ValidateStepResult(AioTestStep step, CommandResult commandResult)
        {
            // ExpectedResult와 실제 결과 비교
            return commandResult.Result?.Contains(step.ExpectedResult) == true;
        }

        public void Stop()
        {
            // 실행 중인 테스트 중단 로직
        }

        private void OnProgressChanged(TestProgressEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
    }
}
