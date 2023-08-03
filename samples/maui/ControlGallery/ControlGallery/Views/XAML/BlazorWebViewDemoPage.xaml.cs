using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class BlazorWebViewDemoPage : ContentPage
    {
        public BlazorWebViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static BlazorWebViewDemoPage Example() => new BlazorWebViewDemoPage();
#endif
    }
}