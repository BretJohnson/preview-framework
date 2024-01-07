// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using ExampleFramework.TestAdapter.ObjectModel;

namespace ExampleFramework.TestAdapter.Extensions;

public static class UnitTestOutcomeExtensions
{
    /// <summary>
    /// Converts the test framework's UnitTestOutcome object to adapter's UnitTestOutcome object.
    /// </summary>
    /// <param name="frameworkTestOutcome">The test framework's UnitTestOutcome object.</param>
    /// <returns>The adapter's UnitTestOutcome object.</returns>
    public static UnitTestOutcome ToUnitTestOutcome(this FrameworkUnitTestOutcome frameworkTestOutcome)
        => frameworkTestOutcome switch
        {
            FrameworkUnitTestOutcome.Failed => UnitTestOutcome.Failed,
            FrameworkUnitTestOutcome.Inconclusive => UnitTestOutcome.Inconclusive,
            FrameworkUnitTestOutcome.InProgress => UnitTestOutcome.InProgress,
            FrameworkUnitTestOutcome.Passed => UnitTestOutcome.Passed,
            FrameworkUnitTestOutcome.Timeout => UnitTestOutcome.Timeout,
            FrameworkUnitTestOutcome.NotRunnable => UnitTestOutcome.NotRunnable,
            _ => UnitTestOutcome.Error,
        };

    /// <summary>
    /// Returns more important outcome of two.
    /// </summary>
    /// <param name="outcome1"> First outcome that needs to be compared. </param>
    /// <param name="outcome2"> Second outcome that needs to be compared. </param>
    /// <returns> Outcome which has higher importance.</returns>
    internal static FrameworkUnitTestOutcome GetMoreImportantOutcome(this FrameworkUnitTestOutcome outcome1, FrameworkUnitTestOutcome outcome2)
    {
        var unitTestOutcome1 = outcome1.ToUnitTestOutcome();
        var unitTestOutcome2 = outcome2.ToUnitTestOutcome();
        return unitTestOutcome1 < unitTestOutcome2 ? outcome1 : outcome2;
    }
}
