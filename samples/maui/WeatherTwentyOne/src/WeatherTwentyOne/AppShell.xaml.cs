using System.Diagnostics;
using ExampleFramework.Maui.Pages;
using WeatherTwentyOne.Pages;

namespace WeatherTwentyOne;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        //App.Current.UserAppTheme = AppTheme.Dark;

        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            CurrentItem = PhoneTabs;

        //MainPage = new ExamplesPage();

        //Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }

    async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
    {
        try { 
            await Shell.Current.GoToAsync($"///settings");
        }catch (Exception ex) {
            Debug.WriteLine($"err: {ex.Message}");
        }
    }
}
