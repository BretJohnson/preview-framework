// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !WINDOWS_UWP

namespace ExampleFramework.TestAdapter.PlatformServices.Utilities;

internal class DeploymentUtilityBase
{
    protected const string TestAssemblyConfigFileExtension = ".config";
    protected const string NetAppConfigFile = "App.Config";

    /// <summary>
    /// Prefix for deployment folder to avoid confusions with other folders (like trx attachments).
    /// </summary>
    protected const string DeploymentFolderPrefix = "Deploy";

    internal static string? GetConfigFile(FileUtility fileUtility, string testSource)
    {
        string? configFile = null;

        var assemblyConfigFile = testSource + TestAssemblyConfigFileExtension;
        if (fileUtility.DoesFileExist(assemblyConfigFile))
        {
            // Path to config file cannot be bad: storage is already checked, and extension is valid.
            configFile = testSource + TestAssemblyConfigFileExtension;
        }
        else
        {
            var netAppConfigFile = Path.Combine(Path.GetDirectoryName(testSource)!, NetAppConfigFile);
            if (fileUtility.DoesFileExist(netAppConfigFile))
            {
                configFile = netAppConfigFile;
            }
        }

        return configFile;
    }
}
#endif
