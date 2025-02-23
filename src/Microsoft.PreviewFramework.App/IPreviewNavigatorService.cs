namespace Microsoft.PreviewFramework.App;

public interface IPreviewNavigatorService
{
    public Task NavigateToPreviewAsync(AppPreview preview);
}
