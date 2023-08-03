using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class WebViewDemoPage : ContentPage
    {
        public WebViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static WebViewDemoPage Example() => new WebViewDemoPage();
#endif
    }
}