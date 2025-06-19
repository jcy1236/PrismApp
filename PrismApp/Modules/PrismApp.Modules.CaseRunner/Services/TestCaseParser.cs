using PrismApp.Modules.CaseRunner.Models;
using PrismApp.Modules.CaseRunner.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PrismApp.Modules.CaseRunner.Services
{
    public class TestCaseParser : ITestCaseParser
    {
        //private readonly ILogger<TestCaseParser> _logger;
        private readonly Dictionary<string, IStepParser> _stepParsers;

        public TestCaseParser(IEnumerable<IStepParser> stepParsers/*, ILogger<TestCaseParser> logger, */)
        {
            //_logger = logger;
            _stepParsers = stepParsers.ToDictionary(p => p.ActionType, p => p);
        }

        public AioTestCase ParseAioTestCase(AioTestCase aioTestCase)
        {
            try
            {
                var testCase = new AioTestCase
                {
                    Key = aioTestCase.Key,
                    Title = aioTestCase.Title,
                    Description = aioTestCase.Description,
                    //Status = ParseStatus(aioTestCase.Status.ToString()),
                    //LastExecuted = aioTestCase.LastModifiedOn,
                    Steps = new List<AioStepDto>()
                };

                // AIO TestSteps를 내부 TestStep으로 변환
                foreach (var aioStep in aioTestCase.Steps.OrderBy(s => s.ID))
                {
                    var testStep = ParseStepDescription(aioStep.Step, aioStep.Data, aioStep.ExpectedResult);

                    testStep.ID = aioStep.ID;
                    testCase.Steps.Add(testStep);
                }

                return testCase;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to parse AIO TestCase {TestCaseKey}", aioTestCase.Key);
                Debug.WriteLine(ex.Message);
                throw;
            }
            return null;
        }

        public AioStepDto ParseStepDescription(string description, string testData, string expectedResult)
        {
            var testStep = new AioStepDto
            {
                Step = ExtractAction(description),
                ExpectedResult = expectedResult,
                Data = testData
            };

            // TestData JSON 파싱
            //if (!string.IsNullOrEmpty(testData))
            //{
            //    try
            //    {
            //        var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(testData);
            //        if (dataDict != null)
            //        {
            //            foreach (var kvp in dataDict)
            //            {
            //                testStep.Parameters[kvp.Key] = kvp.Value;
            //            }
            //        }
            //    }
            //    catch
            //    {
            //        // JSON이 아닌 경우 원시 데이터로 저장
            //        testStep.Data = testData;
            //        ParseRawTestData(testData, testStep.Parameters);
            //    }
            //}

            // Description에서 추가 파라미터 추출
            //ParseDescriptionParameters(description, testStep);

            // Timeout 설정
            //if (testStep.Parameters.ContainsKey("timeout"))
            //{
            //    if (int.TryParse(testStep.Parameters["timeout"].ToString(), out int timeoutSeconds))
            //    {
            //        testStep.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            //    }
            //}

            return testStep;
        }

        public List<SimulatorCommand> ParseStepsToCommands(List<AioTestStep> testSteps)
        {
            var commands = new List<SimulatorCommand>();

            foreach (var step in testSteps.OrderBy(s => s.Order))
            {
                if (_stepParsers.TryGetValue(step.Action, out var parser))
                {
                    var command = parser.CreateCommand(step);
                    commands.Add(command);
                }
                else
                {
                    //_logger.LogWarning("No parser found for action: {Action}", step.Action);
                    // 기본 명령 생성
                    commands.Add(CreateGenericCommand(step));
                }
            }

            return commands;
        }

        private string ExtractAction(string description)
        {
            // 설명에서 액션 타입 추출 (패턴 매칭)
            var actionPatterns = new Dictionary<string, string[]>
            {
                ["SENSOR_READ"] = new[] { "센서", "sensor", "read", "측정" },
                ["MOTOR_CONTROL"] = new[] { "모터", "motor", "control", "구동" },
                ["VALVE_CONTROL"] = new[] { "밸브", "valve", "open", "close" },
                ["WAIT"] = new[] { "대기", "wait", "delay", "sleep" },
                ["VERIFY"] = new[] { "확인", "verify", "check", "validate" },
                ["INIT"] = new[] { "초기화", "initialize", "reset", "init" },
                ["SHUTDOWN"] = new[] { "종료", "shutdown", "stop", "terminate" }
            };

            var lowerDesc = description.ToLower();

            foreach (var pattern in actionPatterns)
            {
                if (pattern.Value.Any(keyword => lowerDesc.Contains(keyword)))
                {
                    return pattern.Key;
                }
            }

            return "CUSTOM";
        }

        private void ParseRawTestData(string testData, Dictionary<string, object> parameters)
        {
            // Key=Value 형식 파싱
            var pairs = testData.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    // 타입 추론
                    if (int.TryParse(value, out int intValue))
                        parameters[key] = intValue;
                    else if (double.TryParse(value, out double doubleValue))
                        parameters[key] = doubleValue;
                    else if (bool.TryParse(value, out bool boolValue))
                        parameters[key] = boolValue;
                    else
                        parameters[key] = value;
                }
            }
        }

        private void ParseDescriptionParameters(string description, AioTestStep testStep)
        {
            // 설명에서 [파라미터=값] 패턴 추출
            var parameterPattern = @"\[(\w+)=([^\]]+)\]";
            var matches = Regex.Matches(description, parameterPattern);

            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;

                testStep.Parameters[key] = value;
            }

            // 숫자 값 추출 (예: "10초 대기", "5V 설정")
            var numberPattern = @"(\d+(?:\.\d+)?)\s*([a-zA-Z가-힣]+)";
            var numberMatches = Regex.Matches(description, numberPattern);

            foreach (Match match in numberMatches)
            {
                var value = double.Parse(match.Groups[1].Value);
                var unit = match.Groups[2].Value.ToLower();

                switch (unit)
                {
                    case "초" or "sec" or "seconds":
                        testStep.Parameters["duration"] = value;
                        break;
                    case "v" or "volt" or "볼트":
                        testStep.Parameters["voltage"] = value;
                        break;
                    case "a" or "amp" or "암페어":
                        testStep.Parameters["current"] = value;
                        break;
                    case "rpm":
                        testStep.Parameters["speed"] = value;
                        break;
                }
            }
        }

        private AioStatusDto ParseStatus(string aioStatus)
        {
            return null;
            //return aioStatus?.ToUpper() switch
            //{
            //    "NOT_EXECUTED" => TestStatus.NotExecuted,
            //    "IN_PROGRESS" => TestStatus.InProgress,
            //    "PASS" => TestStatus.Passed,
            //    "FAIL" => TestStatus.Failed,
            //    "BLOCKED" => TestStatus.Blocked,
            //    "SKIP" => TestStatus.Skipped,
            //    _ => TestStatus.NotExecuted
            //};
        }

        private SimulatorCommand CreateGenericCommand(AioTestStep step)
        {
            return new SimulatorCommand
            {
                Action = step.Action,
                Parameters = step.Parameters,
                Timeout = step.Timeout ?? TimeSpan.FromSeconds(30)
            };
        }

        AioTestStep ITestCaseParser.ParseStepDescription(string description, string testData, string expectedResult)
        {
            throw new NotImplementedException();
        }
    }
}
