using VisualTestUtils;
using VisualTestUtils.AppConnector;

namespace Microsoft.PreviewFramework.App;

public interface IPreviewAppService : IAppService
{
    public Task NavigateToPreviewAsync(string componentName, string previewName);
    public Task<ImageSnapshot> GetPreviewSnapshotAsync(string componentName, string previewName);
    public Task<string[]> GetUIComponentPreviewsAsync(string componentName);
}
