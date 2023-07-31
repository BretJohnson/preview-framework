using ExampleFramework.Tooling.Maui.Views;

namespace ExampleFramework.Tooling.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new DesktopMainPage();
        }
    }
}