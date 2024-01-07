// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Globalization;

using ExampleFramework.TestAdapter.Extensions;

using ExampleFramework.TestAdapter.ObjectModel;
using ExampleFramework.TestAdapter.PlatformServices.Interfaces;
using ExampleFramework.Tooling;

using UnitTestOutcome = ExampleFramework.TestAdapter.ObjectModel.UnitTestOutcome;

namespace ExampleFramework.TestAdapter.Execution;

/// <summary>
/// This class is responsible to running tests and converting framework TestResults to adapter TestResults.
/// </summary>
internal class TestMethodRunner
{
    /// <summary>
    /// Test context which needs to be passed to the various methods of the test.
    /// </summary>
    private readonly ITestContext _testContext;

    /// <summary>
    /// TestMethod that needs to be executed.
    /// </summary>
    private readonly UIExample _example;

    /// <summary>
    /// Specifies whether debug traces should be captured or not.
    /// </summary>
    private readonly bool _captureDebugTraces;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestMethodRunner"/> class.
    /// </summary>
    /// <param name="example">
    /// The example to test.
    /// </param>
    /// <param name="testContext">
    /// The test context.
    /// </param>
    /// <param name="captureDebugTraces">
    /// The capture debug traces.
    /// </param>
    public TestMethodRunner(UIExample example, ITestContext testContext, bool captureDebugTraces)
    {
        _example = example;
        _testContext = testContext;
        _captureDebugTraces = captureDebugTraces;
    }

    /// <summary>
    /// Executes a test.
    /// </summary>
    /// <returns>The test results.</returns>
    internal UnitTestResult[] Execute()
    {
        string? initializationLogs = string.Empty;
        string? initializationTrace = string.Empty;
        string? initializationErrorLogs = string.Empty;
        string? initializationTestContextMessages = string.Empty;

        UnitTestResult[]? result = null;

        try
        {
            // TODO: Add initialization code here if needed for some scenario
#if false
            using (LogMessageListener logListener = new(_captureDebugTraces))
            {
                try
                {
                    // Run the assembly and class Initialize methods if required.
                    // Assembly or class initialize can throw exceptions in which case we need to ensure that we fail the test.
                    _testMethodInfo.Parent.Parent.RunAssemblyInitialize(_testContext.Context);
                    _testMethodInfo.Parent.RunClassInitialize(_testContext.Context);
                }
                finally
                {
                    initializationLogs = logListener.GetAndClearStandardOutput();
                    initializationTrace = logListener.GetAndClearDebugTrace();
                    initializationErrorLogs = logListener.GetAndClearStandardError();
                    initializationTestContextMessages = _testContext.GetAndClearDiagnosticMessages();
                }
            }
#endif

            // Listening to log messages when running the test method with its Test Initialize and cleanup later on in the stack.
            // This allows us to differentiate logging when data driven methods are used.
            result = this.RunExampleTest();
        }
        catch (TestFailedException ex)
        {
            result = new[] { new UnitTestResult(ex) };
        }
        catch (Exception ex)
        {
            if (result == null || result.Length == 0)
            {
                result = new[] { new UnitTestResult() };
            }

            var newResult = new UnitTestResult(new TestFailedException(UnitTestOutcome.Error, ex.TryGetMessage(), ex.TryGetStackTraceInformation()))
            {
                StandardOut = result[result.Length - 1].StandardOut,
                StandardError = result[result.Length - 1].StandardError,
                DebugTrace = result[result.Length - 1].DebugTrace,
                TestContextMessages = result[result.Length - 1].TestContextMessages,
                Duration = result[result.Length - 1].Duration,
            };
            result[result.Length - 1] = newResult;
        }
        finally
        {
            UnitTestResult firstResult = result![0];
            firstResult.StandardOut = initializationLogs + firstResult.StandardOut;
            firstResult.StandardError = initializationErrorLogs + firstResult.StandardError;
            firstResult.DebugTrace = initializationTrace + firstResult.DebugTrace;
            firstResult.TestContextMessages = initializationTestContextMessages + firstResult.TestContextMessages;
        }

        return result;
    }

    /// <summary>
    /// Runs the test method.
    /// </summary>
    /// <returns>The test results.</returns>
    internal UnitTestResult[] RunExampleTest()
    {
        List<FrameworkTestResult> results = new();
        var parentStopwatch = Stopwatch.StartNew();

        FrameworkTestResult[] testResults = this.ExecuteTest(_example);

        foreach (FrameworkTestResult testResult in testResults)
        {
            if (string.IsNullOrWhiteSpace(testResult.DisplayName))
            {
                testResult.DisplayName = _example.GetMethodDisplayName();
            }
        }

        results.AddRange(testResults);

        // Get aggregate outcome.
        FrameworkUnitTestOutcome aggregateOutcome = GetAggregateOutcome(results);
        _testContext.SetOutcome(aggregateOutcome);

        // Set a result in case no result is present.
        if (results.Count == 0)
        {
            FrameworkTestResult emptyResult = new()
            {
                Outcome = aggregateOutcome,
                TestFailureException = new TestFailedException(UnitTestOutcome.Error, Resource.UTA_NoTestResult),
            };

            results.Add(emptyResult);
        }

        UnitTestResult[] unitTestResults = results
            .ToArray()
            .ToUnitTestResults();

        return unitTestResults;
    }

    private FrameworkTestResult[] ExecuteTest(UIExample example)
    {
        try
        {
            Debug.WriteLine($"Executing test for {example.FullName}");

            return new[]
            {
                new FrameworkTestResult()
                {
                    TestFailureException = new NotImplementedException($"Testing support not yet implemented; example: {example.FullName}")
                }
            };
        }
        catch (Exception ex)
        {
            return new[]
            {
                new FrameworkTestResult()
                {
                    TestFailureException = new Exception(string.Format(CultureInfo.CurrentCulture, Resource.UTA_ExecuteThrewException, ex?.Message, ex?.StackTrace), ex),
                },
            };
        }
    }

    /// <summary>
    /// Gets aggregate outcome.
    /// </summary>
    /// <param name="results">Results.</param>
    /// <returns>Aggregate outcome.</returns>
    private static FrameworkUnitTestOutcome GetAggregateOutcome(List<FrameworkTestResult> results)
    {
        // In case results are not present, set outcome as unknown.
        if (results.Count == 0)
        {
            return FrameworkUnitTestOutcome.Unknown;
        }

        // Get aggregate outcome.
        FrameworkUnitTestOutcome aggregateOutcome = results[0].Outcome;
        foreach (FrameworkTestResult result in results)
        {
            aggregateOutcome = UnitTestOutcomeExtensions.GetMoreImportantOutcome(aggregateOutcome, result.Outcome);
        }

        return aggregateOutcome;
    }
}
