using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class TimePickerDemoPage : ContentPage
    {
        public TimePickerDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static TimePickerDemoPage Example() => new TimePickerDemoPage();
#endif
    }
}