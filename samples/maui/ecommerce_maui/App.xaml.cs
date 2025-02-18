using EcommerceMAUI.Views;
using ExampleFramework.App.Maui;

namespace EcommerceMAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Current.UserAppTheme = AppTheme.Light;
        MainPage = new LoginView();

#if EXAMPLES
        this.EnableExamplesMode();
#endif
    }
}
