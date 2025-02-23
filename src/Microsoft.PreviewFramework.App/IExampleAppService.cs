using VisualTestUtils;
using VisualTestUtils.AppConnector;

namespace Microsoft.PreviewFramework.App;

public interface IExampleAppService : IAppService
{
    public Task NavigateToExampleAsync(string componentName, string exampleName);
    public Task<ImageSnapshot> GetExampleSnapshotAsync(string componentName, string exampleName);
    public Task<string[]> GetUIComponentExamplesAsync(string componentName);
}
