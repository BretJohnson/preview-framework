using Microsoft.PreviewFramework.App;
using Microsoft.PreviewFramework.Maui.Views;
using VisualTestUtils;

namespace Microsoft.PreviewFramework.Maui;

public class MauiPreviewAppService : PreviewAppService
{
    public MauiPreviewAppService()
    {
        this.PreviewNavigatorService = new MauiPreviewNavigatorService();
    }

    public async override Task NavigateToPreviewAsync(string uiComponentName, string previewName)
    {
        AppPreview preview = GetPreview(uiComponentName, previewName);
        await this.PreviewNavigatorService.NavigateToPreviewAsync(preview).ConfigureAwait(false);
    }

    public IPreviewNavigatorService PreviewNavigatorService { get; }

    public async override Task<ImageSnapshot> GetPreviewSnapshotAsync(string uiComponentName, string previewName)
    {
        AppPreview preview = GetPreview(uiComponentName, previewName);

        if (Application.Current?.MainPage is not RemoteControlMainPage remoteControlMainPage)
            throw new InvalidOperationException("MainPage isn't a RemoteControlMainPage");

        return await remoteControlMainPage.GetPreviewSnapshotAsync(preview);
    }
}
