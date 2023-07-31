using ExampleFramework.Tooling.Maui;

namespace PointOfSale;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        App.Current.UserAppTheme = AppTheme.Dark;

        if (true /* DeviceInfo.Idiom == DeviceIdiom.Phone */)
        {
            MainPage = new AppShellMobile();
        }
        else
        {
            MainPage = new AppShell();
        }
    }
}
