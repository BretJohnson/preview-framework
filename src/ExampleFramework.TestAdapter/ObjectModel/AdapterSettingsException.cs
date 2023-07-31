// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace ExampleFramework.TestAdapter.ObjectModel;

internal class AdapterSettingsException : Exception
{
    internal AdapterSettingsException(string? message)
        : base(message)
    {
    }
}
