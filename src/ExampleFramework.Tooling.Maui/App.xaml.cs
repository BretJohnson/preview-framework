using ExampleFramework.Tooling.Maui.Views;

namespace ExampleFramework.Tooling.Maui
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            //MainPage = new AppShell();
            this.MainPage = new DesktopGalleryMainPage();
        }
    }
}
