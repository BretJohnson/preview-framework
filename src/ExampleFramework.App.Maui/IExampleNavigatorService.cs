namespace ExampleFramework.App.Maui;

public class MauiExampleNavigatorService : IExampleNavigatorService
{
    public async Task NavigateToExampleAsync(AppUIExample example)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            object? exampleUI = example.Create();

            //MauiExamplesApplication.Instance.PrepareToNavigateToExample();

            if (exampleUI is ShellExample shellExample)
            {
                await Shell.Current.GoToAsync(shellExample.Route, animate:false, shellExample.Parameters);
            }
            else if (exampleUI is ContentPage contentPage)
            {
                //MauiExamplesApplication.Instance.Application.MainPage = contentPage;
                await Application.Current.MainPage.Navigation.PushAsync(contentPage);
            }
        });
    }
}
