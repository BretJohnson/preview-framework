using ExampleFramework.Tooling.Maui.Views;

namespace ExampleFramework.Tooling.Maui;

public static class ExamplesManagerMaui
{
    private static Window? _window = null;

    public static void Noop()
    {
    }

    public static bool LaunchInExamplesMode(Application app, string? galleryAppTitle, string? environmentVariable)
    {
#if true
        ExamplesMode examplesMode;
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            examplesMode = ExamplesMode.Gallery;
        else examplesMode = ExamplesMode.RemoteControl;
#else
        ExamplesMode examplesMode = GetExamplesMode(environmentVariable);
#endif

        if (examplesMode == ExamplesMode.None)
            return false;

        if (examplesMode == ExamplesMode.Gallery)
        {
            app.MainPage = new DesktopGalleryMainPage();

            IReadOnlyList<Window> appWindows = app.Windows;
            if (appWindows.Count > 0)
                appWindows[0].Title = galleryAppTitle;

            return true;
        }
        else
        {
            app.MainPage = new RemoteControlMainPage();
            return true;
        }
    }

    public static ExamplesMode GetExamplesMode(string? environmentVariable = null)
    {
        if (environmentVariable != null)
        {
            string? value = Environment.GetEnvironmentVariable(environmentVariable);
            if (value != null && !string.Equals(value, "0") && !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
                return ExamplesMode.Gallery;
        }

        string[] args = Environment.GetCommandLineArgs();

        int count = args.Length;
        for (int i = 0; i < count; i++)
        {
            string arg = args[i];
            if (string.Equals(arg, "--examples-gallery", StringComparison.OrdinalIgnoreCase))
            {
                return ExamplesMode.Gallery;
            }
            else if (string.Equals(arg, "--examples-remote-control", StringComparison.OrdinalIgnoreCase))
            {
                return ExamplesMode.RemoteControl;
            }
        }

        return ExamplesMode.None;
    }
}
