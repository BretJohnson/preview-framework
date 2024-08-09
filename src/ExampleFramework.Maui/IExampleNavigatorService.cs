using ExampleFramework.Tooling;

namespace ExampleFramework.Maui;

public class MauiExampleNavigatorService : IExampleNavigatorService
{
    public async Task NavigateToExampleAsync(UIExample example)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            object? exampleUI = example.Create();

            MauiExamplesApplication.Instance.PrepareToNavigateToExample();

            if (exampleUI is ShellExample shellExample)
            {
                await Shell.Current.GoToAsync(shellExample.Route, animate:false, shellExample.Parameters);
            }
            else if (exampleUI is ContentPage contentPage)
            {
                await MauiExamplesApplication.Instance.Application.MainPage!.Navigation.PushAsync(contentPage);
            }
        });
    }
}
