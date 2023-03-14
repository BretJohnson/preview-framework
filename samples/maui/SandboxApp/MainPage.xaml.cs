using ExampleBook;
using ExampleBook.Tooling.Maui;
using ExampleBook.Tooling.Maui.Views;

namespace SandboxApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnShowClicked(object sender, EventArgs e)
        {
            ExamplesManagerDesktopMaui.ShowWindow();
        }

#if DEBUG
        [Example("CategoryA/Example1")]
        public static MainPage Example1() => new MainPage();

        [Example("CategoryA/Example2")]
        public static MainPage Example2() => new MainPage();
#endif
    }
}