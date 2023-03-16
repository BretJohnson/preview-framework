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
            ExamplesManagerMaui.ShowWindow();
        }

#if DEBUG
        [UIExample("Example1")]
        public static MainPage Example1() => new MainPage();

        [UIExample("Example1/Example1LightMode")]
        public static MainPage Example2() => new MainPage();

        [UIExample("Example1/Example1DarkMode")]
        public static MainPage Example3() => new MainPage();
#endif
    }
}