using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class VerticalStackLayoutDemoPage : ContentPage
    {
        public VerticalStackLayoutDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static VerticalStackLayoutDemoPage Example() => new VerticalStackLayoutDemoPage();
#endif
    }
}