using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class DatePickerDemoPage : ContentPage
    {
        public DatePickerDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static DatePickerDemoPage Example() => new DatePickerDemoPage();
#endif
    }
}