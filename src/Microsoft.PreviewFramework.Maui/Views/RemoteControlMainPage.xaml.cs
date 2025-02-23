using Microsoft.PreviewFramework.App;
using VisualTestUtils;

namespace Microsoft.PreviewFramework.Maui.Views;

public partial class RemoteControlMainPage : ContentPage
{
	public RemoteControlMainPage()
	{
        this.InitializeComponent();
    }

    public async Task SetExampleAsync(AppUIExample example)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            object? exampleUI = example.Create();

            if (exampleUI is ContentPage contentPage)
            {
                this.ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
                this.ExampleWrapper.Content = contentPage.Content;
                this.ExampleWrapper.BindingContext = contentPage.BindingContext;
            }
            else if (exampleUI is View view)
            {
                this.ExampleWrapper.Content = view;
            }
            else
            {
                this.ExampleWrapper.Content = null;
            }
        });
    }

    public async Task<ImageSnapshot> GetExampleSnapshotAsync(AppUIExample example)
    {
        return await MainThread.InvokeOnMainThreadAsync<ImageSnapshot>(async () =>
        {
            object? exampleUI = example.Create();

            if (exampleUI is ContentPage contentPage)
            {
                this.ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
                this.ExampleWrapper.Content = contentPage.Content;
                this.ExampleWrapper.BindingContext = contentPage.BindingContext;

                return await this.GetExampleWrapperSnapshotAsync();

            }
            else if (exampleUI is View view)
            {
                this.ExampleWrapper.Content = view;
                return await this.GetExampleWrapperSnapshotAsync();
            }
            else if (exampleUI is null)
            {
                throw new InvalidOperationException($"Example {example.Name} returned null");
            }
            else
            {
                throw new InvalidOperationException($"Example type {exampleUI.GetType().FullName} returned from {example.Name} is not supported");
            }
        });
    }

    private async Task<ImageSnapshot> GetExampleWrapperSnapshotAsync()
    {
        byte[]? data = await VisualDiagnostics.CaptureAsPngAsync(this.ExampleWrapper);
        if (data == null)
        {
            throw new InvalidOperationException("VisualDiagnostics.CaptureAsPngAsync failed, returning null");
        }

        return new ImageSnapshot(data, ImageSnapshotFormat.PNG);
    }
}
