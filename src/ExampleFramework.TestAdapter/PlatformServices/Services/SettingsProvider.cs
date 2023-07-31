// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using ExampleFramework.TestAdapter.PlatformServices.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ExampleFramework.TestAdapter.PlatformServices.Services;

/// <summary>
/// Class to read settings from the runsettings xml for the desktop.
/// </summary>
public class TestSettingsProvider : ISettingsProvider
{
#if !WINDOWS_UWP
    /// <summary>
    /// Member variable for Adapter settings.
    /// </summary>
    private static TestAdapterSettings? s_settings;

    /// <summary>
    /// Gets settings provided to the adapter.
    /// </summary>
    public static TestAdapterSettings Settings
    {
        get
        {
            s_settings ??= new TestAdapterSettings();

            return s_settings;
        }
    }

    /// <summary>
    /// Reset the settings to its default.
    /// </summary>
    public static void Reset()
    {
        s_settings = null;
    }
#endif

    /// <summary>
    /// Load the settings from the reader.
    /// </summary>
    /// <param name="reader">Reader to load the settings from.</param>
    public void Load(XmlReader reader)
    {
#if !WINDOWS_UWP
        ValidateArg.NotNull(reader, "reader");
        s_settings = TestAdapterSettings.ToSettings(reader);
#endif
    }
}
