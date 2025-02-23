using VisualTestUtils;
using VisualTestUtils.AppConnector.App;

namespace Microsoft.PreviewFramework.App;

public abstract class PreviewAppService : AppService, IPreviewAppService
{
    public abstract Task NavigateToPreviewAsync(string uiComponentName, string previewName);
    public abstract Task<ImageSnapshot> GetPreviewSnapshotAsync(string uiComponentName, string previewName);

    protected static AppUIComponent GetUIComponent(string uiComponentName)
    {
        AppUIComponents uiComponents = PreviewsManager.Instance.UIComponents;

        AppUIComponent? uiComponent = uiComponents.GetUIComponent(uiComponentName);
        if (uiComponent == null)
        {
            throw new UIComponentNotFoundException($"UIComponent {uiComponentName} not found");
        }

        return uiComponent;
    }

    public Task<string[]> GetUIComponentPreviewsAsync(string componentName)
    {
        AppUIComponent component = GetUIComponent(componentName);
        string[] previewNames = component.Previews.Select(preview => preview.Name).ToArray();

        return Task.FromResult(previewNames);
    }

    protected static AppPreview GetPreview(string uiComponentName, string previewName)
    {
        AppUIComponent uiComponent = GetUIComponent(uiComponentName);

        AppPreview? preview = uiComponent.GetPreview(previewName);
        if (preview == null)
        {
            throw new PreviewNotFoundException($"Preview {previewName} not found for UIComponent {uiComponentName}");
        }

        return preview;
    }
}
