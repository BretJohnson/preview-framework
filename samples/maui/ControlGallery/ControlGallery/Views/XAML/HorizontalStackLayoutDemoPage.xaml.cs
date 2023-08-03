using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class HorizontalStackLayoutDemoPage : ContentPage
    {
        public HorizontalStackLayoutDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static HorizontalStackLayoutDemoPage Example() => new HorizontalStackLayoutDemoPage();
#endif
    }
}