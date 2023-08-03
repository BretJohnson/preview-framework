using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class GridDemoPage : ContentPage
    {
        public GridDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static GridDemoPage Example() => new GridDemoPage();
#endif
    }
}