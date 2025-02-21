using System.Diagnostics;
using ExampleFramework.App.Maui;
using ExampleFramework.App.Maui.Pages;
using WeatherTwentyOne.Pages;

namespace WeatherTwentyOne;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

#if EXAMPLES
        this.EnableExamplesMode();
#endif

        /*
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            Shell.Current.CurrentItem = appShell.PhoneTabs;
        */
    }
}
