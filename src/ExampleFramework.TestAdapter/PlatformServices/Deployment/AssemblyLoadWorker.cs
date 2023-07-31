// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using ExampleFramework.TestAdapter.Internal;
using ExampleFramework.TestAdapter.PlatformServices.Utilities;

namespace ExampleFramework.TestAdapter.PlatformServices.Deployment;

/*
 * /!\ WARNING /!\
 * DO NOT USE EQTTRACE IN THIS CLASS AS IT WILL CAUSE LOAD ISSUE BECAUSE OF THE APPDOMAIN
 * ASSEMBLY RESOLVER SETUP.
 */

/// <summary>
/// Utility function for Assembly related info
/// The caller is supposed to create AppDomain and create instance of given class in there.
/// </summary>
internal class AssemblyLoadWorker : MarshalByRefObject
{
    private readonly IAssemblyUtility _assemblyUtility;

    public AssemblyLoadWorker()
        : this(new AssemblyUtility())
    {
    }

    internal AssemblyLoadWorker(IAssemblyUtility assemblyUtility)
    {
        _assemblyUtility = assemblyUtility;
    }

    /// <summary>
    /// initialize the lifetime service.
    /// </summary>
    /// <returns> The <see cref="object"/>. </returns>
    public override object? InitializeLifetimeService()
    {
        // Infinite.
        return null;
    }

    /// <summary>
    /// Get the target dotNet framework string for the assembly.
    /// </summary>
    /// <param name="path">Path of the assembly file.</param>
    /// <returns> String representation of the target dotNet framework e.g. .NETFramework,Version=v4.0. </returns>
    internal string GetTargetFrameworkVersionStringFromPath(string path, out string? errorMessage)
    {
        errorMessage = null;
        if (!File.Exists(path))
        {
            return string.Empty;
        }

        try
        {
            Assembly a = _assemblyUtility.ReflectionOnlyLoadFrom(path);
            return GetTargetFrameworkStringFromAssembly(a);
        }
        catch (BadImageFormatException)
        {
            errorMessage = "AssemblyHelper:GetTargetFrameworkVersionString() caught BadImageFormatException. Falling to native binary.";
        }
        catch (Exception ex)
        {
            errorMessage = $"AssemblyHelper:GetTargetFrameworkVersionString() Returning default. Unhandled exception: {ex}.";
        }

        return string.Empty;
    }

    /// <summary>
    /// Get the target dot net framework string for the assembly.
    /// </summary>
    /// <param name="assembly">Assembly from which target framework has to find.</param>
    /// <returns>String representation of the target dot net framework e.g. .NETFramework,Version=v4.0. </returns>
    private static string GetTargetFrameworkStringFromAssembly(Assembly assembly)
    {
        string dotNetVersion = string.Empty;
        foreach (CustomAttributeData data in CustomAttributeData.GetCustomAttributes(assembly))
        {
            if (!(data?.NamedArguments?.Count > 0))
            {
                continue;
            }

            var declaringType = data.NamedArguments[0].MemberInfo.DeclaringType;
            if (declaringType == null)
            {
                continue;
            }

            string attributeName = declaringType.FullName;
            if (string.Equals(
                attributeName,
                Constants.TargetFrameworkAttributeFullName,
                StringComparison.OrdinalIgnoreCase))
            {
                dotNetVersion = data.ConstructorArguments[0].Value.ToString();
                break;
            }
        }

        return dotNetVersion;
    }
}

#endif
