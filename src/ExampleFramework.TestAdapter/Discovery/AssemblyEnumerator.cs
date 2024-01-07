// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;
using ExampleFramework.TestAdapter.Internal;
using ExampleFramework.TestAdapter.ObjectModel;
using ExampleFramework.Tooling;

namespace ExampleFramework.TestAdapter.Discovery;

/// <summary>
/// Enumerates through all types in the assembly in search of valid test methods.
/// </summary>
internal class AssemblyEnumerator : MarshalByRefObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyEnumerator"/> class.
    /// </summary>
    public AssemblyEnumerator()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyEnumerator"/> class.
    /// </summary>
    /// <param name="settings">The settings for the session.</param>
    /// <remarks>Use this constructor when creating this object in a new app domain so the settings for this app domain are set.</remarks>
    public AssemblyEnumerator(TestSettings settings)
    {
        // Populate the settings into the domain(Desktop workflow) performing discovery.
        // This would just be resetting the settings to itself in non desktop workflows.
        TestSettings.PopulateSettings(settings);
    }

    /// <summary>
    /// Gets or sets the run settings to use for current discovery session.
    /// </summary>
    public string? RunSettingsXml { get; set; }

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
    /// Enumerates through all types in the assembly in search of valid test methods.
    /// </summary>
    /// <param name="assemblyFileName">The assembly file name.</param>
    /// <param name="warnings">Contains warnings if any, that need to be passed back to the caller.</param>
    /// <returns>A collection of Test Elements.</returns>
    internal ICollection<TestElement> EnumerateAssembly(string assemblyFileName, out ICollection<string> warnings)
    {
        DebugEx.Assert(!string.IsNullOrWhiteSpace(assemblyFileName), "Invalid assembly file name.");

        var warningMessages = new List<string>();

        Assembly assembly = PlatformServiceProvider.Instance.FileOperations.LoadAssembly(assemblyFileName, isReflectionOnly: false);

        UIComponents uiComponents = new UIComponents();
        uiComponents.AddFromAssembly(assembly);

        var testElements = new List<TestElement>();
        foreach (UIComponent uiComponent in uiComponents.Components)
        {
            foreach (UIExample uiExample in uiComponent.Examples)
            {
                testElements.Add(new TestElement(uiExample, assemblyFileName));
            }
        }

        /*
        foreach (var type in types)
        {
            if (type == null)
            {
                continue;
            }

            var testsInType = DiscoverTestsInType(assemblyFileName, RunSettingsXml, type, warningMessages, discoverInternals,
                testDataSourceDiscovery);
            tests.AddRange(testsInType);
        }
        */

        warnings = warningMessages;
        return testElements;
    }

    /// <summary>
    /// Gets the types defined in an assembly.
    /// </summary>
    /// <param name="assembly">The reflected assembly.</param>
    /// <param name="assemblyFileName">The file name of the assembly.</param>
    /// <param name="warningMessages">Contains warnings if any, that need to be passed back to the caller.</param>
    /// <returns>Gets the types defined in the provided assembly.</returns>
    internal static Type?[] GetTypes(Assembly assembly, string assemblyFileName, ICollection<string>? warningMessages)
    {
        var types = new List<Type>();
        try
        {
            types.AddRange(assembly.DefinedTypes.Select(typeInfo => typeInfo.AsType()));
        }
        catch (ReflectionTypeLoadException ex)
        {
            PlatformServiceProvider.Instance.AdapterTraceLogger.LogWarning($"MSTestExecutor.TryGetTests: {Resource.TestAssembly_AssemblyDiscoveryFailure}", assemblyFileName, ex);
            PlatformServiceProvider.Instance.AdapterTraceLogger.LogWarning(Resource.ExceptionsThrown);

            if (ex.LoaderExceptions != null)
            {
                // If not able to load all type, log a warning and continue with loaded types.
                var message = string.Format(CultureInfo.CurrentCulture, Resource.TypeLoadFailed, assemblyFileName, GetLoadExceptionDetails(ex));

                warningMessages?.Add(message);

                foreach (var loaderEx in ex.LoaderExceptions)
                {
                    PlatformServiceProvider.Instance.AdapterTraceLogger.LogWarning("{0}", loaderEx);
                }
            }

            return ex.Types!;
        }

        return types.ToArray();
    }

    /// <summary>
    /// Formats load exception as multi-line string, each line contains load error message.
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <returns>Returns loader exceptions as a multi-line string.</returns>
    internal static string GetLoadExceptionDetails(ReflectionTypeLoadException ex)
    {
        var map = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase); // Exception -> null.
        var errorDetails = new StringBuilder();

        if (ex.LoaderExceptions?.Length > 0)
        {
            // Loader exceptions can contain duplicates, leave only unique exceptions.
            foreach (var loaderException in ex.LoaderExceptions)
            {
                DebugEx.Assert(loaderException != null, "loader exception should not be null.");
                var line = string.Format(CultureInfo.CurrentCulture, Resource.EnumeratorLoadTypeErrorFormat, loaderException.GetType(), loaderException.Message);
                if (!map.ContainsKey(line))
                {
                    map.Add(line, null);
                    errorDetails.AppendLine(line);
                }
            }
        }
        else
        {
            errorDetails.AppendLine(ex.Message);
        }

        return errorDetails.ToString();
    }
}
