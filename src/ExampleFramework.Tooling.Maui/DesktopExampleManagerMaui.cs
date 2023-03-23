using ExampleFramework.Tooling.Maui.Views;

namespace ExampleFramework.Tooling.Maui;

public static class ExamplesManagerMaui
{
    private static Window? _window = null;

    public static bool LaunchInExamplesMode(Application app, string? environmentVariable = null)
    {
        if (! ShouldLaunchInExamplesMode(environmentVariable))
            return false;

        if (true /* isDesktop */)
        {
            var desktopMainPage = new DesktopMainPage();
            //desktopMainPage.BindingContext = new UIExamplesManagerDesktopViewModel(CurrentAppUIExamplesManager.Instance);

            app.MainPage = desktopMainPage;
            return true;
        }
    }

    public static bool ShouldLaunchInExamplesMode(string? environmentVariable = null)
    {
        if (environmentVariable != null)
        {
            string? value = Environment.GetEnvironmentVariable(environmentVariable);
            if (value != null && !string.Equals(value, "0") && !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
                return true;
        }

        string[] args = Environment.GetCommandLineArgs();

        int count = args.Length;
        for (int i = 0; i < count; i++)
        {
            string arg = args[i];
            if (string.Equals(arg, "-examplesmode", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public static void ShowWindow()
    {
        if (_window == null)
        {
            var desktopMainPage = new DesktopMainPage();
            //desktopMainPage.BindingContext = new UIExamplesManagerDesktopViewModel(CurrentAppUIExamplesManager.Instance);

            _window = new Window(desktopMainPage);
            Application.Current.OpenWindow(_window);
        }
    }
}