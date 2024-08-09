using ExampleFramework.Maui.Views;
using ExampleFramework.Tooling;
using VisualTestUtils;

namespace ExampleFramework.Maui;

public class MauiExampleAppService : ExampleAppService
{
    public async override Task NavigateToExampleAsync(string componentName, string? exampleName)
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
