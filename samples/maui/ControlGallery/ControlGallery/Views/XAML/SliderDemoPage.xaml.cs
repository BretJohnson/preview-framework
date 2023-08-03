using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class SliderDemoPage : ContentPage
    {
        public SliderDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static SliderDemoPage Example() => new SliderDemoPage();
#endif
    }
}