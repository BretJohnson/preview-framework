// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ExampleFramework.TestAdapter;

/// <summary>
/// unit test outcomes. This enum was named UnitTestOutcome in the original MSTest adapter code -
/// there were two UnitTestCome types in the original code, this one (part of the public MSTest
/// framework API apparently) and the one used by the adapter (which is still called
/// UnitTestOutcome in this code). This enum may get cleaned up/removed at some point.
/// </summary>
public enum FrameworkUnitTestOutcome : int
{
    /// <summary>
    /// Test was executed, but there were issues.
    /// Issues may involve exceptions or failed assertions.
    /// </summary>
    Failed,

    /// <summary>
    /// Test has completed, but we can't say if it passed or failed.
    /// May be used for aborted tests.
    /// </summary>
    Inconclusive,

    /// <summary>
    /// Test was executed without any issues.
    /// </summary>
    Passed,

    /// <summary>
    /// Test is currently executing.
    /// </summary>
    InProgress,

    /// <summary>
    /// There was a system error while we were trying to execute a test.
    /// </summary>
    Error,

    /// <summary>
    /// The test timed out.
    /// </summary>
    Timeout,

    /// <summary>
    /// Test was aborted by the user.
    /// </summary>
    Aborted,

    /// <summary>
    /// Test is in an unknown state.
    /// </summary>
    Unknown,

    /// <summary>
    /// Test cannot be executed.
    /// </summary>
    NotRunnable,
}
