using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class LabelDemoPage : ContentPage
    {
        public LabelDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static LabelDemoPage Example() => new LabelDemoPage();
#endif
    }
}