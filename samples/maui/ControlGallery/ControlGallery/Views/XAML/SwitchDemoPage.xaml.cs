using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class SwitchDemoPage : ContentPage
    {
        public SwitchDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static SwitchDemoPage Example() => new SwitchDemoPage();
#endif
    }
}