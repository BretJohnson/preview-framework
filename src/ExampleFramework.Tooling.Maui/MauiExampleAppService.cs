using ExampleFramework.Tooling.Maui.Views;
using VisualTestUtils;

namespace ExampleFramework.Tooling.Maui;

public class MauiExampleAppService : ExampleAppService
{
    public async override Task NaviateToExampleAsync(string componentName, string? exampleName)
    {
        UIExample example = GetExample(componentName, exampleName);

        if (Application.Current?.MainPage is not RemoteControlMainPage remoteControlMainPage)
            throw new InvalidOperationException("MainPage isn't a RemoteControlMainPage");

        await remoteControlMainPage.SetExampleAsync(example);
    }

    public async override Task<ImageSnapshot> GetExampleSnapshotAsync(string componentName, string? exampleName)
    {
        UIExample example = GetExample(componentName, exampleName);

        if (Application.Current?.MainPage is not RemoteControlMainPage remoteControlMainPage)
            throw new InvalidOperationException("MainPage isn't a RemoteControlMainPage");

        return await remoteControlMainPage.GetExampleSnapshotAsync(example);
    }
}
