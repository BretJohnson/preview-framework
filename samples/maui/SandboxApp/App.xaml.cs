using ExampleFramework.Tooling.Maui;

namespace SandboxApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}