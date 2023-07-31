using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExampleFramework.Tooling.Maui;

public static class MauiAppManager
{
    public static void EnableExamplesMode<TApp>(MauiAppBuilder builder) where TApp : class, IApplication
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
            return;

        if (examplesMode == ExamplesMode.Gallery)
        {
            // Remove the existing Application service, so we can replace it with ours
            builder.Services.RemoveAll<IApplication>();

            builder.UseMauiApp<App>();

#if DESKTOP_PLATFORM
            builder.UseMauiApp<ExampleFramework.Tooling.Maui.DesktopGallery.App>();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents(options =>
            {
                options!.HostingModel = BlazorHostingModel.Hybrid;
            });
            builder.Services.AddScoped<IStaticAssetService, FileBasedStaticAssetService>();
#endif
        }
        else
        {
            //app.MainPage = new RemoteControlMainPage();
            //return true;
        }
    }

#if OLD
    public static bool LaunchInExamplesMode(string? galleryAppTitle, string? environmentVariable)
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
            app.MainPage = new DesktopMainPage();

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
#endif

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
