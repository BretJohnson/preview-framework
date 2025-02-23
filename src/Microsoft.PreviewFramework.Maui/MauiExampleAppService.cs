using Microsoft.PreviewFramework.App;
using Microsoft.PreviewFramework.Maui.Views;
using VisualTestUtils;

namespace Microsoft.PreviewFramework.Maui;

public class MauiExampleAppService : ExampleAppService
{
    public MauiExampleAppService()
    {
        this.ExampleNavigatorService = new MauiExampleNavigatorService();
    }

    public async override Task NavigateToExampleAsync(string uiComponentName, string exampleName)
    {
        AppUIExample example = GetExample(uiComponentName, exampleName);
        await this.ExampleNavigatorService.NavigateToExampleAsync(example).ConfigureAwait(false);
    }

    public IExampleNavigatorService ExampleNavigatorService { get; }

    public async override Task<ImageSnapshot> GetExampleSnapshotAsync(string uiComponentName, string exampleName)
    {
        AppUIExample example = GetExample(uiComponentName, exampleName);

        if (Application.Current?.MainPage is not RemoteControlMainPage remoteControlMainPage)
            throw new InvalidOperationException("MainPage isn't a RemoteControlMainPage");

        return await remoteControlMainPage.GetExampleSnapshotAsync(example);
    }
}
