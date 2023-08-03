using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class StepperDemoPage : ContentPage
    {
        public StepperDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static StepperDemoPage Example() => new StepperDemoPage();
#endif
    }
}