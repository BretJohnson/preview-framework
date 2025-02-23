using System.Diagnostics;
using Microsoft.PreviewFramework.Maui;
using Microsoft.PreviewFramework.Maui.Pages;
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
