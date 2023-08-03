using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class RelativeLayoutDemoPage : ContentPage
    {
        public RelativeLayoutDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static RelativeLayoutDemoPage Example() => new RelativeLayoutDemoPage();
#endif
    }
}