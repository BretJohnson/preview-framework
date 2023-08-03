using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class StackLayoutDemoPage : ContentPage
    {
        public StackLayoutDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static StackLayoutDemoPage Example() => new StackLayoutDemoPage();
#endif
    }
}