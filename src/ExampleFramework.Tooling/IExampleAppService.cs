using VisualTestUtils;
using VisualTestUtils.AppConnector;

namespace ExampleFramework.Tooling;

public interface IExampleAppService : IAppService
{
    public Task NaviateToExampleAsync(string componentName, string? exampleName);
    public Task<ImageSnapshot> GetExampleSnapshotAsync(string componentName, string? exampleName);
    public Task<string[]> GetUIComponentExamplesAsync(string componentName);
}
