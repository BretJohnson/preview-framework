// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Reflection;
using ExampleFramework.TestAdapter.ObjectModel;
using ExampleFramework.Tooling;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace ExampleFramework.TestAdapter.Discovery;

internal class ExampleTestDiscoverer
{
    private readonly AssemblyEnumeratorWrapper _assemblyEnumeratorWrapper;

    internal ExampleTestDiscoverer()
    {
        _assemblyEnumeratorWrapper = new AssemblyEnumeratorWrapper();
        TestMethodFilter = new TestMethodFilter();
    }

    /// <summary>
    /// Gets or sets method filter for filtering tests.
    /// </summary>
    private TestMethodFilter TestMethodFilter { get; set; }

    /// <summary>
    /// Discovers the tests available from the provided sources.
    /// </summary>
    /// <param name="sources"> The sources. </param>
    /// <param name="logger"> The logger. </param>
    /// <param name="discoverySink"> The discovery Sink. </param>
    /// <param name="discoveryContext"> The discovery context. </param>
    internal void DiscoverTests(
        IEnumerable<string> sources,
        IMessageLogger logger,
        ITestCaseDiscoverySink discoverySink,
        IDiscoveryContext? discoveryContext)
    {
        foreach (string source in sources)
        {
            DiscoverTestsInSource(source, logger, discoverySink, discoveryContext);
        }
    }

    /// <summary>
    /// Get the tests from the parameter source.
    /// </summary>
    /// <param name="source"> The source. </param>
    /// <param name="logger"> The logger. </param>
    /// <param name="discoverySink"> The discovery Sink. </param>
    /// <param name="discoveryContext"> The discovery context. </param>
    internal virtual void DiscoverTestsInSource(
        string source,
        IMessageLogger logger,
        ITestCaseDiscoverySink discoverySink,
        IDiscoveryContext? discoveryContext)
    {
        ICollection<TestElement>? testElements = _assemblyEnumeratorWrapper.GetTests(source, discoveryContext?.RunSettings, out var warnings);

        var treatDiscoveryWarningsAsErrors = TestSettings.CurrentSettings.TreatDiscoveryWarningsAsErrors;

        // log the warnings
        foreach (var warning in warnings)
        {
            PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo(
                "MSTestDiscoverer: Warning during discovery from {0}. {1} ",
                source,
                warning);
            var message = string.Format(CultureInfo.CurrentCulture, Resource.DiscoveryWarning, source, warning);
            logger.SendMessage(treatDiscoveryWarningsAsErrors ? TestMessageLevel.Error : TestMessageLevel.Warning, message);
        }

        // No tests found => nothing to do
        if (testElements == null || testElements.Count == 0)
        {
            return;
        }

        PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo(
            "MSTestDiscoverer: Found {0} UIComponents from source {1}",
            testElements.Count,
            source);

        SendTestCases(source, testElements, discoverySink, discoveryContext, logger);
    }

    internal void SendTestCases(string source, IEnumerable<TestElement> testElements, ITestCaseDiscoverySink discoverySink, IDiscoveryContext? discoveryContext, IMessageLogger logger)
    {
        var shouldCollectSourceInformation = TestSettings.RunConfigurationSettings.CollectSourceInformation;

        var navigationSessions = new Dictionary<string, object?>();
        try
        {
            if (shouldCollectSourceInformation)
            {
                navigationSessions.Add(source, PlatformServiceProvider.Instance.FileOperations.CreateNavigationSession(source));
            }

            // Get filter expression and skip discovery in case filter expression has parsing error.
            ITestCaseFilterExpression? filterExpression = TestMethodFilter.GetFilterExpression(discoveryContext, logger, out var filterHasError);
            if (filterHasError)
            {
                return;
            }

            foreach (TestElement testElement in testElements)
            {
                TestCase testCase = testElement.ToTestCase();

                // Filter tests based on test case filters
                if (filterExpression != null && filterExpression.MatchTestCase(testCase, (p) => TestMethodFilter.PropertyValueProvider(testCase, p)) == false)
                {
                    continue;
                }

                if (!shouldCollectSourceInformation)
                {
                    discoverySink.SendTestCase(testCase);
                    continue;
                }

                if (!navigationSessions.TryGetValue(source, out var testNavigationSession))
                {
                    testNavigationSession = PlatformServiceProvider.Instance.FileOperations.CreateNavigationSession(source);
                    navigationSessions.Add(source, testNavigationSession);
                }

                if (testNavigationSession == null)
                {
                    discoverySink.SendTestCase(testCase);
                    continue;
                }

                MethodInfo methodInfo = testElement.Example.MethodInfo;

                string className = methodInfo.DeclaringType?.FullName ?? "";
                string methodName = methodInfo.Name;

#if false
                // If it is async test method use compiler generated type and method name for navigation data.
                if (!string.IsNullOrEmpty(uiComponent.AsyncTypeName))
                {
                    className = uiComponent.AsyncTypeName;

                    // compiler generated method name is "MoveNext".
                    methodName = "MoveNext";
                }
#endif

                PlatformServiceProvider.Instance.FileOperations.GetNavigationData(
                    testNavigationSession,
                    className,
                    methodName,
                    out var minLineNumber,
                    out var fileName);

                if (!string.IsNullOrEmpty(fileName))
                {
                    testCase.LineNumber = minLineNumber;
                    testCase.CodeFilePath = fileName;
                }

                discoverySink.SendTestCase(testCase);
            }
        }
        finally
        {
            foreach (object? navigationSession in navigationSessions.Values)
            {
                PlatformServiceProvider.Instance.FileOperations.DisposeNavigationSession(navigationSession);
            }
        }
    }
}
