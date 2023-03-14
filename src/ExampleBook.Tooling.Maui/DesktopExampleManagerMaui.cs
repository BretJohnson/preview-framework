using ExampleBook.Tooling.Maui.ViewModels;
using ExampleBook.Tooling.Maui.Views;

namespace ExampleBook.Tooling.Maui;

public static class ExamplesManagerDesktopMaui
{
    private static Window _window = null;

    public static void ShowWindow()
    {
        if (_window == null)
        {
            var desktopMainPage = new DesktopMainPage();
            desktopMainPage.BindingContext = new UIExamplesManagerDesktopViewModel(CurrentAppUIExamplesManager.Instance);

            _window = new Window(desktopMainPage);
            Application.Current.OpenWindow(_window);
        }
    }
}