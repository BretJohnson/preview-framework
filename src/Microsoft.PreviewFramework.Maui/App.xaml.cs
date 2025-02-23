using Microsoft.PreviewFramework.Maui.Pages;

namespace Microsoft.PreviewFramework.Maui;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        //MainPage = new AppShell();
        this.MainPage = new PreviewsPage();
    }
}
