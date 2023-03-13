using ExampleBook;
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
            Application.Current.OpenWindow(new Window(new DesktopMainPage()));
        }

/* Non working example examples */
#if false 
#if DEBUG
        [Example("CategoryA/Example1")]
        public static MainPage Example1() => new MainPage(new MyViewModel("example1data"));

        [Example("CategoryA/Example2")]
        public static MainPage Example2() => new MainPage(new MyViewModel("example2data"));
#endif
#endif
    }
}