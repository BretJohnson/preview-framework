using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class FrameDemoPage : ContentPage
    {
        public FrameDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static FrameDemoPage Example() => new FrameDemoPage();
#endif
    }
}