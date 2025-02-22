using ExampleFramework.App.Maui.Pages;

namespace ExampleFramework.App.Maui;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        //MainPage = new AppShell();
        this.MainPage = new ExamplesPage();
    }
}
