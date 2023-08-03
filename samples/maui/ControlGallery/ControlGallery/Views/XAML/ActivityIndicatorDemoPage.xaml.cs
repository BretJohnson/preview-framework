using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ActivityIndicatorDemoPage : ContentPage
    {
        public ActivityIndicatorDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ActivityIndicatorDemoPage Example() => new ActivityIndicatorDemoPage();
#endif
    }
}