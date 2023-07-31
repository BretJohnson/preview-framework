using ExampleFramework.TestAdapter.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System.Diagnostics;

namespace ExampleFramework.TestAdapter;

/// <summary>
/// Contains the discovery logic for this adapter.
/// </summary>
[DefaultExecutorUri(Constants.ExecutorUriString)]
[FileExtension(".xap")]
[FileExtension(".appx")]
[FileExtension(".dll")]
[FileExtension(".exe")]
public class ExampleFrameworkTestDiscoverer : ITestDiscoverer
{
    /// <summary>
    /// Discovers the tests available from the provided source. Not supported for .xap source.
    /// </summary>
    /// <param name="sources">Collection of test containers.</param>
    /// <param name="discoveryContext">Context in which discovery is being performed.</param>
    /// <param name="logger">Logger used to log messages.</param>
    /// <param name="discoverySink">Used to send testcases and discovery related events back to Discoverer manager.</param>
    [System.Security.SecurityCritical]
    public void DiscoverTests(
        IEnumerable<string> sources,
        IDiscoveryContext? discoveryContext,
        IMessageLogger logger,
        ITestCaseDiscoverySink discoverySink)
    {
        // Debugger.Launch();

        if (!AreValidSources(sources))
        {
            throw new NotSupportedException(Resource.SourcesNotSupported);
        }

        new ExampleTestDiscoverer().DiscoverTests(sources, logger, discoverySink, discoveryContext);
    }

    /// <summary>
    /// Verifies if the sources are valid for the target platform.
    /// </summary>
    /// <param name="sources">The test sources.</param>
    /// <remarks>Sources cannot be null.</remarks>
    /// <returns>True if the source has a valid extension for the current platform.</returns>
    internal static bool AreValidSources(IEnumerable<string> sources)
    {
        // ValidSourceExtensions is always expected to return a non-null list.
        return
            sources.Any(
                source =>
                PlatformServiceProvider.Instance.TestSource.ValidSourceExtensions.Any(
                    extension =>
                    string.Equals(Path.GetExtension(source), extension, StringComparison.OrdinalIgnoreCase)));
    }
}
