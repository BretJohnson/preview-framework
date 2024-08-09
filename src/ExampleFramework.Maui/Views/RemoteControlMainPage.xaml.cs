using ExampleFramework.Tooling;
using VisualTestUtils;

namespace ExampleFramework.Maui.Views;

public partial class RemoteControlMainPage : ContentPage
{
	public RemoteControlMainPage()
	{
        this.InitializeComponent();
    }

    public async Task SetExampleAsync(UIExample example)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            object? exampleUI = example.Create();

            if (exampleUI is ContentPage contentPage)
            {
                ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
                ExampleWrapper.Content = contentPage.Content;
                ExampleWrapper.BindingContext = contentPage.BindingContext;
            }
            else if (exampleUI is View view)
            {
                ExampleWrapper.Content = view;
            }
            else
            {
                ExampleWrapper.Content = null;
            }
        });
    }

    public async Task<ImageSnapshot> GetExampleSnapshotAsync(UIExample example)
    {
        return await MainThread.InvokeOnMainThreadAsync<ImageSnapshot>(async () =>
        {
            object? exampleUI = example.Create();

            if (exampleUI is ContentPage contentPage)
            {
                ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
                ExampleWrapper.Content = contentPage.Content;
                ExampleWrapper.BindingContext = contentPage.BindingContext;

                return await this.GetExampleWrapperSnapshotAsync();

            }
            else if (exampleUI is View view)
            {
                ExampleWrapper.Content = view;
                return await this.GetExampleWrapperSnapshotAsync();
            }
            else if (exampleUI is null)
            {
                throw new InvalidOperationException($"Example {example.FullName} returned null");
            }
            else
            {
                throw new InvalidOperationException($"Example type {exampleUI.GetType().FullName} returned from {example.FullName} is not supported");
            }
        });
    }

    private async Task<ImageSnapshot> GetExampleWrapperSnapshotAsync()
    {
        byte[]? data = await VisualDiagnostics.CaptureAsPngAsync(ExampleWrapper);
        if (data == null)
        {
            throw new InvalidOperationException("VisualDiagnostics.CaptureAsPngAsync failed, returning null");
        }

        return new ImageSnapshot(data, ImageSnapshotFormat.PNG);
    }
}
