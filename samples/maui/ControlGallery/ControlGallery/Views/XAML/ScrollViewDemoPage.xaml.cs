using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ScrollViewDemoPage : ContentPage
    {
        public ScrollViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ScrollViewDemoPage Example() => new ScrollViewDemoPage();
#endif
    }
}