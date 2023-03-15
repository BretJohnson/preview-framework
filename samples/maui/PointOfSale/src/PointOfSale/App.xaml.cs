using ExampleBook.Tooling.Maui;

namespace PointOfSale;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();

        App.Current.UserAppTheme = AppTheme.Dark;

#if DEBUG
        if (ExamplesManagerMaui.LaunchInExamplesMode(this, "EXAMPLES_MODE"))
        {
            return;
        }
#endif

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