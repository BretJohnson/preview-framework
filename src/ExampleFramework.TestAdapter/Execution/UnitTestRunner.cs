// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Globalization;
using System.Security;

using ExampleFramework.TestAdapter.Extensions;
using ExampleFramework.TestAdapter.ObjectModel;
using ExampleFramework.TestAdapter.PlatformServices;
using ExampleFramework.TestAdapter.PlatformServices.Interfaces;
using ExampleFramework.Tooling;

namespace ExampleFramework.TestAdapter.Execution;

/// <summary>
/// The runner that runs a single unit test. Also manages the assembly and class cleanup methods at the end of the run.
/// </summary>
internal class UnitTestRunner : MarshalByRefObject
{
    /// <summary>
    /// Class cleanup manager.
    /// </summary>
    private ClassCleanupManager? _classCleanupManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitTestRunner"/> class.
    /// </summary>
    /// <param name="settings"> Specifies adapter settings. </param>
    internal UnitTestRunner(TestSettings settings)
    {
        // Populate the settings into the domain(Desktop workflow) performing discovery.
        // This would just be resetting the settings to itself in non desktop workflows.
        TestSettings.PopulateSettings(settings);
    }

    /// <summary>
    /// Returns object to be used for controlling lifetime, null means infinite lifetime.
    /// </summary>
    /// <returns>
    /// The <see cref="object"/>.
    /// </returns>
    [SecurityCritical]
#if NET5_0_OR_GREATER
    [Obsolete]
#endif
    public override object InitializeLifetimeService()
    {
        return null!;
    }

    /// <summary>
    /// Initialized the class cleanup manager for the unit test runner. Note, this can run over process-isolation,
    /// and all inputs must be serializable from host process.
    /// </summary>
    /// <param name="testsToRun">the list of tests that will be run in this execution.</param>
    internal void InitializeClassCleanupManager(ICollection<TestElement> testsToRun)
    {
        _classCleanupManager = new ClassCleanupManager(testsToRun);
    }

    /// <summary>
    /// Runs a single test.F
    /// </summary>
    /// <param name="example"> The test Method. </param>
    /// <param name="testContextProperties"> The test context properties. </param>
    /// <returns> The <see cref="UnitTestResult"/>. </returns>
    internal UnitTestResult[] RunSingleTest(UIExample example, IDictionary<string, object?> testContextProperties)
    {
        using var writer = new ThreadSafeStringWriter(CultureInfo.InvariantCulture, "context");
        Dictionary<string, object?> properties = new Dictionary<string, object?>(testContextProperties);
        ITestContext testContext = PlatformServiceProvider.Instance.GetTestContext(example, writer, properties);
        testContext.SetOutcome(FrameworkUnitTestOutcome.InProgress);

        if (!this.IsTestMethodRunnable(example, out UnitTestResult[]? notRunnableResult))
        {
            if (_classCleanupManager is null)
            {
                return notRunnableResult!;
            }

            this.RunRequiredCleanups(testContext, example, notRunnableResult);

            return notRunnableResult;
        }

        var testMethodRunner = new TestMethodRunner(example, testContext, TestSettings.CurrentSettings.CaptureDebugTraces);
        UnitTestResult[] result = testMethodRunner.Execute();
        this.RunRequiredCleanups(testContext, example, result);
        return result;
    }

    private void RunRequiredCleanups(ITestContext testContext, UIExample example, UnitTestResult[] results)
    {
        bool shouldRunClassCleanup = false;
        bool shouldRunClassAndAssemblyCleanup = false;
        _classCleanupManager?.MarkTestComplete(example, out shouldRunClassCleanup, out shouldRunClassAndAssemblyCleanup);

        // TODO: Perhaps add a cleanup mechanism later
#if false
        using LogMessageListener logListener = new(TestSettings.CurrentSettings.CaptureDebugTraces);

        try
        {
            if (shouldRunClassCleanup)
            {
                testMethodInfo.Parent.ExecuteClassCleanup();
            }

            if (shouldRunClassAndAssemblyCleanup)
            {
                ImmutableArray<TestClassInfo> classInfoCache = _typeCache.ClassInfoListWithExecutableCleanupMethods;
                foreach (TestClassInfo classInfo in classInfoCache)
                {
                    classInfo.ExecuteClassCleanup();
                }

                ImmutableArray<TestAssemblyInfo> assemblyInfoCache = _typeCache.AssemblyInfoListWithExecutableCleanupMethods;
                foreach (TestAssemblyInfo assemblyInfo in assemblyInfoCache)
                {
                    assemblyInfo.ExecuteAssemblyCleanup();
                }
            }
        }
        catch (Exception ex)
        {
            // We mainly expect TestFailedException here as each cleanup method is executed in a try-catch block but
            // for the sake of the catch-all mechanism, let's keep it as Exception.
            if (results.Length == 0)
            {
                return;
            }

            UnitTestResult lastResult = results[results.Length - 1];
            lastResult.Outcome = ObjectModel.UnitTestOutcome.Failed;
            lastResult.ErrorMessage = ex.Message;
            lastResult.ErrorStackTrace = ex.StackTrace;
        }
        finally
        {
            var cleanupTestContextMessages = testContext.GetAndClearDiagnosticMessages();

            if (results.Length > 0)
            {
                UnitTestResult lastResult = results[results.Length - 1];
                lastResult.StandardOut += logListener.StandardOutput;
                lastResult.StandardError += logListener.StandardError;
                lastResult.DebugTrace += logListener.DebugTrace;
                lastResult.TestContextMessages += cleanupTestContextMessages;
            }
        }
#endif
    }

    /// <summary>
    /// Whether the given testMethod is runnable.
    /// </summary>
    /// <param name="example">The testMethod.</param>
    /// <param name="notRunnableResult">The results to return if the test method is not runnable.</param>
    /// <returns>whether the given testMethod is runnable.</returns>
    private bool IsTestMethodRunnable(
        UIExample example,
        out UnitTestResult[]? notRunnableResult)
    {
        // TODO: Add a mechanism to ignore Example tests
        notRunnableResult = null;
        return true;
#if false
        // If the specified TestMethod could not be found, return a NotFound result.
        if (testMethodInfo == null)
        {
            {
                notRunnableResult = new UnitTestResult[]
                {
                    new UnitTestResult(
                        ObjectModel.UnitTestOutcome.NotFound,
                        string.Format(CultureInfo.CurrentCulture, Resource.TestNotFound, testMethod.Name)),
                };
                return false;
            }
        }

        // If test cannot be executed, then bail out.
        if (!testMethodInfo.IsRunnable)
        {
            {
                notRunnableResult = new UnitTestResult[]
                {
                    new UnitTestResult(ObjectModel.UnitTestOutcome.NotRunnable, testMethodInfo.NotRunnableReason),
                };
                return false;
            }
        }

        string? ignoreMessage = null;
        var isIgnoreAttributeOnClass =
            _reflectHelper.IsAttributeDefined<IgnoreAttribute>(testMethodInfo.Parent.ClassType, false);
        var isIgnoreAttributeOnMethod =
            _reflectHelper.IsAttributeDefined<IgnoreAttribute>(testMethodInfo.TestMethod, false);

        if (isIgnoreAttributeOnClass)
        {
            ignoreMessage = _reflectHelper.GetIgnoreMessage(testMethodInfo.Parent.ClassType.GetTypeInfo());
        }

        if (string.IsNullOrEmpty(ignoreMessage) && isIgnoreAttributeOnMethod)
        {
            ignoreMessage = _reflectHelper.GetIgnoreMessage(testMethodInfo.TestMethod);
        }

        if (isIgnoreAttributeOnClass || isIgnoreAttributeOnMethod)
        {
            {
                notRunnableResult = new[] { new UnitTestResult(ObjectModel.UnitTestOutcome.Ignored, ignoreMessage) };
                return false;
            }
        }

        notRunnableResult = null;
        return true;
#endif
    }

    private class ClassCleanupManager
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _remainingTestsByClass;

        public ClassCleanupManager(IEnumerable<TestElement> testsToRun)
        {
            _remainingTestsByClass =
                new(testsToRun.GroupBy(t => t.Example.MethodInfo.DeclaringType.FullName)
                    .ToDictionary(
                        g => g.Key,
                        g => new HashSet<string>(g.Select(t => t.Example.FullName))));
        }

        public void MarkTestComplete(UIExample example, out bool shouldRunEndOfClassCleanup,
            out bool shouldRunEndOfAssemblyCleanup)
        {
            string testClassName = example.MethodInfo.DeclaringType.FullName;

            shouldRunEndOfClassCleanup = false;
            shouldRunEndOfAssemblyCleanup = false;
            if (!_remainingTestsByClass.TryGetValue(testClassName, out HashSet<string>? testsByClass))
            {
                return;
            }

            lock (testsByClass)
            {
                testsByClass.Remove(example.FullName);
                if (testsByClass.Count == 0)
                {
                    _remainingTestsByClass.TryRemove(testClassName, out _);
                    // TODO: Perhaps add cleanup support later
#if false
                    if (testMethodInfo.Parent.HasExecutableCleanupMethod)
                    {
                        shouldRunEndOfClassCleanup = true;
                    }
#endif
                }

                shouldRunEndOfAssemblyCleanup = _remainingTestsByClass.IsEmpty;
            }
        }
    }
}
