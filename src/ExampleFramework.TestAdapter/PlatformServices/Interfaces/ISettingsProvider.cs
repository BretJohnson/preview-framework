// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace ExampleFramework.TestAdapter.PlatformServices.Interfaces;

/// <summary>
/// To read settings from the runsettings xml for the corresponding platform service.
/// </summary>
public interface ISettingsProvider
{
    /// <summary>
    /// Load settings from the xml reader instance which are specific
    /// for the corresponding platform service.
    /// </summary>
    /// <param name="reader">
    /// Reader that can be used to read current node and all its descendants,
    /// to load the settings from.</param>
    void Load(XmlReader reader);
}
