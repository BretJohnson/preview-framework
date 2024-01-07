// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ExampleFramework.TestAdapter.Discovery;
using ExampleFramework.TestAdapter.Extensions;
using ExampleFramework.TestAdapter.Helpers;
using ExampleFramework.TestAdapter.Internal;
using ExampleFramework.TestAdapter.Lifecycle;
using ExampleFramework.TestAdapter.ObjectModel;
using ExampleFramework.TestAdapter.PlatformServices.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace ExampleFramework.TestAdapter.Execution;

/// <summary>
/// Class responsible for execution of tests at assembly level and sending tests via framework handle.
/// </summary>
public class TestExecutionManager
{
    /// <summary>
    /// Dictionary for test run parameters.
    /// </summary>
    private readonly IDictionary<string, object> _sessionParameters;
    private readonly IEnvironment _environment;
    private readonly Func<Action, Task> _taskFactory;

    /// <summary>
    /// Specifies whether the test run is canceled or not.
    /// </summary>
    private TestRunCancellationToken? _cancellationToken;

    public TestExecutionManager()
        : this(new EnvironmentWrapper())
    {
    }

    internal TestExecutionManager(IEnvironment environment, Func<Action, Task>? taskFactory = null)
    {
        this.TestMethodFilter = new TestMethodFilter();
        _sessionParameters = new Dictionary<string, object>();
        _environment = environment;
        _taskFactory = taskFactory
            ?? (action => Task.Factory.StartNew(
                action,
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default));
    }

    /// <summary>
    /// Gets or sets method filter for filtering tests.
    /// </summary>
    private TestMethodFilter TestMethodFilter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether any test executed has failed.
    /// </summary>
    private bool HasAnyTestFailed { get; set; }

    /// <summary>
    /// Runs the tests.
    /// </summary>
    /// <param name="tests">Tests to be run.</param>
    /// <param name="runContext">Context to use when executing the tests.</param>
    /// <param name="frameworkHandle">Handle to the framework to record results and to do framework operations.</param>
    /// <param name="runCancellationToken">Test run cancellation token.</param>
    public void RunTests(IEnumerable<TestCase> tests, IRunContext? runContext, IFrameworkHandle frameworkHandle, TestRunCancellationToken runCancellationToken)
    {
        DebugEx.Assert(tests != null, "tests");
        DebugEx.Assert(runContext != null, "runContext");
        DebugEx.Assert(frameworkHandle != null, "frameworkHandle");
        DebugEx.Assert(runCancellationToken != null, "runCancellationToken");

        _cancellationToken = runCancellationToken;

        // TODO: For now, the app must already be running & connected; later we may do automatic deployment
#if false
        var isDeploymentDone = PlatformServiceProvider.Instance.TestDeployment.Deploy(tests, runContext, frameworkHandle);
#else
        var isDeploymentDone = true;
#endif

        // Placing this after deployment since we need information post deployment that we pass in as properties.
        this.CacheSessionParameters(runContext, frameworkHandle!);

        // Execute the tests
        this.ExecuteTests(tests!, runContext, frameworkHandle!, isDeploymentDone);

        if (!this.HasAnyTestFailed)
        {
        // TODO: For now, the app must already be running & connected; later we may do automatic deployment
#if false
            PlatformServiceProvider.Instance.TestDeployment.Cleanup();
#endif
        }
    }

    public void RunTests(IEnumerable<string> sources, IRunContext? runContext, IFrameworkHandle frameworkHandle, TestRunCancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;

        var discoverySink = new TestCaseDiscoverySink();

        var tests = new List<TestCase>();

        // deploy everything first.
        foreach (var source in sources)
        {
            if (_cancellationToken.Canceled)
            {
                break;
            }

            var logger = (IMessageLogger)frameworkHandle;

            // discover the tests
            this.GetUnitTestDiscoverer().DiscoverTestsInSource(source, logger, discoverySink, runContext);
            tests.AddRange(discoverySink.Tests);

            // Clear discoverSinksTests so that it just stores test for one source at one point of time
            discoverySink.Tests.Clear();
        }

        // TODO: For now, the app must already be running & connected; later we may do automatic deployment
#if false
        bool isDeploymentDone = PlatformServiceProvider.Instance.TestDeployment.Deploy(tests, runContext, frameworkHandle);
#else
        bool isDeploymentDone = true;
#endif

        // Placing this after deployment since we need information post deployment that we pass in as properties.
        this.CacheSessionParameters(runContext, frameworkHandle);

        // Run tests.
        this.ExecuteTests(tests, runContext, frameworkHandle, isDeploymentDone);

        if (!this.HasAnyTestFailed)
        {
        // TODO: For now, the app must already be running & connected; later we may do automatic deployment
#if false
            PlatformServiceProvider.Instance.TestDeployment.Cleanup();
#endif
        }
    }

    /// <summary>
    /// Execute the parameter tests.
    /// </summary>
    /// <param name="tests">Tests to execute.</param>
    /// <param name="runContext">The run context.</param>
    /// <param name="frameworkHandle">Handle to record test start/end/results.</param>
    /// <param name="isDeploymentDone">Indicates if deployment is done.</param>
    internal virtual void ExecuteTests(IEnumerable<TestCase> tests, IRunContext? runContext, IFrameworkHandle frameworkHandle, bool isDeploymentDone)
    {
        var testsBySource = from test in tests
                            group test by test.Source into testGroup
                            select new { Source = testGroup.Key, Tests = testGroup };

        foreach (var group in testsBySource)
        {
            this.ExecuteTestsInSource(group.Tests, runContext, frameworkHandle, group.Source, isDeploymentDone);
        }
    }

    internal virtual ExampleTestDiscoverer GetUnitTestDiscoverer()
    {
        return new();
    }

    internal void SendTestResults(TestCase test, UnitTestResult[] unitTestResults, DateTimeOffset startTime, DateTimeOffset endTime, ITestExecutionRecorder testExecutionRecorder)
    {
        foreach (UnitTestResult unitTestResult in unitTestResults)
        {
            if (test == null)
            {
                continue;
            }

            var testResult = unitTestResult.ToTestResult(test, startTime, endTime, _environment.MachineName, TestSettings.CurrentSettings);

            if (unitTestResult.DatarowIndex >= 0)
            {
                testResult.DisplayName = string.Format(CultureInfo.CurrentCulture, Resource.DataDrivenResultDisplayName, test.DisplayName, unitTestResult.DatarowIndex);
            }

            testExecutionRecorder.RecordEnd(test, testResult.Outcome);

            if (testResult.Outcome == TestOutcome.Failed)
            {
                PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("ExampleFrameworkTestExecutor:Test {0} failed. ErrorMessage:{1}, ErrorStackTrace:{2}.", testResult.TestCase.FullyQualifiedName, testResult.ErrorMessage, testResult.ErrorStackTrace);
                this.HasAnyTestFailed = true;
            }

            try
            {
                testExecutionRecorder.RecordResult(testResult);
            }
            catch (TestCanceledException)
            {
                // Ignore this exception
            }
        }
    }

    private static bool MatchTestFilter(ITestCaseFilterExpression? filterExpression, TestCase test, TestMethodFilter testMethodFilter)
    {
        if (filterExpression != null && filterExpression.MatchTestCase(test, p => testMethodFilter.PropertyValueProvider(test, p)) == false)
        {
            // Skip test if not fitting filter criteria.
            return false;
        }

        return true;
    }

    /// <summary>
    /// Execute the parameter tests present in parameter source.
    /// </summary>
    /// <param name="tests">Tests to execute.</param>
    /// <param name="runContext">The run context.</param>
    /// <param name="frameworkHandle">Handle to record test start/end/results.</param>
    /// <param name="source">The test container for the tests.</param>
    /// <param name="isDeploymentDone">Indicates if deployment is done.</param>
    private void ExecuteTestsInSource(IEnumerable<TestCase> tests, IRunContext? runContext, IFrameworkHandle frameworkHandle, string source, bool isDeploymentDone)
    {
        DebugEx.Assert(!string.IsNullOrEmpty(source), "Source cannot be empty");

        if (isDeploymentDone)
        {
        // TODO: For now, the app must already be running & connected; later we may do automatic deployment
#if false
            source = Path.Combine(PlatformServiceProvider.Instance.TestDeployment.GetDeploymentDirectory()!, Path.GetFileName(source));
#endif
        }

        using ITestSourceHost isolationHost = PlatformServiceProvider.Instance.CreateTestSourceHost(source, runContext?.RunSettings, frameworkHandle);

        // Create an instance of a type defined in adapter so that adapter gets loaded in the child app domain
        var testRunner = (UnitTestRunner)isolationHost.CreateInstanceForType(
            typeof(UnitTestRunner),
            new object[] { TestSettings.CurrentSettings })!;

        PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Created unit-test runner {0}", source);

        // Default test set is filtered tests based on user provided filter criteria
        ICollection<TestCase> testsToRun = Array.Empty<TestCase>();
        ITestCaseFilterExpression? filterExpression = this.TestMethodFilter.GetFilterExpression(runContext, frameworkHandle, out var filterHasError);
        if (filterHasError)
        {
            // Bail out without processing everything else below.
            return;
        }

        testsToRun = tests.Where(t => MatchTestFilter(filterExpression, t, this.TestMethodFilter)).ToArray();

        // this is done so that appropriate values of test context properties are set at source level
        // and are merged with session level parameters
        var sourceLevelParameters = PlatformServiceProvider.Instance.SettingsProvider.GetProperties(source);

        if (_sessionParameters != null && _sessionParameters.Count > 0)
        {
            sourceLevelParameters = _sessionParameters.ConcatWithOverwrites(sourceLevelParameters);
        }

        TestAssemblySettingsProvider? sourceSettingsProvider = null;

        try
        {
            sourceSettingsProvider = isolationHost.CreateInstanceForType(
                typeof(TestAssemblySettingsProvider),
                null) as TestAssemblySettingsProvider;
        }
        catch (Exception ex)
        {
            PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Could not create TestAssemblySettingsProvider instance in child app-domain", ex);
        }

        var sourceSettings = (sourceSettingsProvider != null) ? TestAssemblySettingsProvider.GetSettings(source) : new TestAssemblySettings();
        var parallelWorkers = sourceSettings.Workers;
        var parallelScope = sourceSettings.Scope;
        InitializeClassCleanupManager(source, testRunner, testsToRun, sourceSettings);

        if (TestSettings.CurrentSettings.ParallelizationWorkers.HasValue)
        {
            // The runsettings value takes precedence over an assembly level setting. Reset the level.
            parallelWorkers = TestSettings.CurrentSettings.ParallelizationWorkers.Value;
        }

        if (TestSettings.CurrentSettings.ParallelizationScope.HasValue)
        {
            // The runsettings value takes precedence over an assembly level setting. Reset the level.
            parallelScope = TestSettings.CurrentSettings.ParallelizationScope.Value;
        }

        if (!TestSettings.CurrentSettings.DisableParallelization && sourceSettings.CanParallelizeAssembly && parallelWorkers > 0)
        {
            // Parallelization is enabled. Let's do further classification for sets.
            var logger = (IMessageLogger)frameworkHandle;
            logger.SendMessage(
                TestMessageLevel.Informational,
                string.Format(CultureInfo.CurrentCulture, Resource.TestParallelizationBanner, source, parallelWorkers, parallelScope));

            // Create test sets for execution, we can execute them in parallel based on parallel settings
            IEnumerable<IGrouping<bool, TestCase>> testSets = Enumerable.Empty<IGrouping<bool, TestCase>>();

            // Parallel and not parallel sets.
            testSets = testsToRun.GroupBy(t => t.GetPropertyValue(TestAdapter.Constants.DoNotParallelizeProperty, false));

            var parallelizableTestSet = testSets.FirstOrDefault(g => g.Key == false);
            var nonParallelizableTestSet = testSets.FirstOrDefault(g => g.Key == true);

            if (parallelizableTestSet != null)
            {
                ConcurrentQueue<IEnumerable<TestCase>>? queue = null;

                // Chunk the sets into further groups based on parallel level
                switch (parallelScope)
                {
                    case ExecutionScope.MethodLevel:
                        queue = new ConcurrentQueue<IEnumerable<TestCase>>(parallelizableTestSet.Select(t => new[] { t }));
                        break;

                    case ExecutionScope.ClassLevel:
                        queue = new ConcurrentQueue<IEnumerable<TestCase>>(parallelizableTestSet.GroupBy(t => t.GetPropertyValue(TestAdapter.Constants.TestClassNameProperty) as string));
                        break;
                }

                var tasks = new List<Task>();

                for (int i = 0; i < parallelWorkers; i++)
                {
                    tasks.Add(_taskFactory(() =>
                    {
                        while (!queue!.IsEmpty)
                        {
                            if (_cancellationToken != null && _cancellationToken.Canceled)
                            {
                                // if a cancellation has been requested, do not queue any more test runs.
                                break;
                            }

                            if (queue.TryDequeue(out IEnumerable<TestCase>? testSet))
                            {
                                this.ExecuteTestsWithTestRunner(testSet, frameworkHandle, source, sourceLevelParameters, testRunner);
                            }
                        }
                    }));
                }

                Task.WaitAll(tasks.ToArray());
            }

            // Queue the non parallel set
            if (nonParallelizableTestSet != null)
            {
                this.ExecuteTestsWithTestRunner(nonParallelizableTestSet, frameworkHandle, source, sourceLevelParameters, testRunner);
            }
        }
        else
        {
            this.ExecuteTestsWithTestRunner(testsToRun, frameworkHandle, source, sourceLevelParameters, testRunner);
        }

        PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Executed tests belonging to source {0}", source);
    }

    private static void InitializeClassCleanupManager(string source, UnitTestRunner testRunner, ICollection<TestCase> testsToRun, TestAssemblySettings sourceSettings)
    {
        try
        {
            TestElement[] unitTestElements = testsToRun.Select(e => e.ToTestElement(source)).ToArray();
            testRunner.InitializeClassCleanupManager(unitTestElements);
        }
        catch (Exception ex)
        {
            // source might not support this if it's legacy make sure it's supported by checking for the type
            if (ex.GetType().FullName != "System.Runtime.Remoting.RemotingException")
            {
                throw;
            }
        }
    }

    private void ExecuteTestsWithTestRunner(
        IEnumerable<TestCase> tests,
        ITestExecutionRecorder testExecutionRecorder,
        string source,
        IDictionary<string, object> sourceLevelParameters,
        UnitTestRunner testRunner)
    {
        foreach (TestCase currentTest in tests)
        {
            if (_cancellationToken != null && _cancellationToken.Canceled)
            {
                break;
            }

            var unitTestElement = currentTest.ToTestElement(source);

            testExecutionRecorder.RecordStart(currentTest);

            DateTimeOffset startTime = DateTimeOffset.Now;

            PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Executing test {0}", unitTestElement.Example.Title);

            // Run single test passing test context properties to it.
            IDictionary<TestProperty, object?> tcmProperties = TcmTestPropertiesProvider.GetTcmProperties(currentTest);
            IDictionary<string, object?> testContextProperties = GetTestContextProperties(tcmProperties, sourceLevelParameters);
            UnitTestResult[] unitTestResult = testRunner.RunSingleTest(unitTestElement.Example, testContextProperties);

            PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Executed test {0}", unitTestElement.Example.Title);

            DateTimeOffset endTime = DateTimeOffset.Now;

            this.SendTestResults(currentTest, unitTestResult, startTime, endTime, testExecutionRecorder);
        }
    }

    /// <summary>
    /// Get test context properties.
    /// </summary>
    /// <param name="tcmProperties">Tcm properties.</param>
    /// <param name="sourceLevelParameters">Source level parameters.</param>
    /// <returns>Test context properties.</returns>
    private static IDictionary<string, object?> GetTestContextProperties(
        IDictionary<TestProperty, object?> tcmProperties,
        IDictionary<string, object> sourceLevelParameters)
    {
        var testContextProperties = new Dictionary<string, object?>();

        // Add tcm properties.
        foreach (KeyValuePair<TestProperty, object?> propertyPair in tcmProperties)
        {
            testContextProperties[propertyPair.Key.Id] = propertyPair.Value;
        }

        // Add source level parameters.
        foreach (KeyValuePair<string, object> propertyPair in sourceLevelParameters)
        {
            testContextProperties[propertyPair.Key] = propertyPair.Value;
        }

        return testContextProperties;
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Requirement is to handle errors in user specified run parameters")]
    private void CacheSessionParameters(IRunContext? runContext, ITestExecutionRecorder testExecutionRecorder)
    {
        string? settingsXml = runContext?.RunSettings?.SettingsXml;
        if (settingsXml == null || string.IsNullOrEmpty(settingsXml))
        {
            return;
        }

        try
        {
            var testRunParameters = RunSettingsUtilities.GetTestRunParameters(settingsXml);
            if (testRunParameters != null)
            {
                // Clear sessionParameters to prevent key collisions of test run parameters in case
                // "Keep Test Execution Engine Alive" is selected in VS.
                _sessionParameters.Clear();
                foreach (var kvp in testRunParameters)
                {
                    _sessionParameters.Add(kvp);
                }
            }
        }
        catch (Exception ex)
        {
            testExecutionRecorder.SendMessage(TestMessageLevel.Error, ex.Message);
        }
    }
}
