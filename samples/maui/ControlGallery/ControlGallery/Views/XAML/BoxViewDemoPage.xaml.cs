using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class BoxViewDemoPage : ContentPage
    {
        public BoxViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static BoxViewDemoPage Example() => new BoxViewDemoPage();
#endif
    }
}