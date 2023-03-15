using ExampleBook.Tooling.Maui;

namespace SandboxApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

#if DEBUG
            if (ExamplesManagerMaui.LaunchInExamplesMode(this, "EXAMPLES_MODE"))
            {
                return;
            }
#endif

            MainPage = new AppShell();
        }
    }
}