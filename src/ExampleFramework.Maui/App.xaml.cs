using ExampleFramework.Maui.Pages;

namespace ExampleFramework.Maui;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        //MainPage = new AppShell();
        this.MainPage = new ExamplesPage();
    }
}
