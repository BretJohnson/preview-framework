using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ImageCellDemoPage : ContentPage
    {
        public ImageCellDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ImageCellDemoPage Example() => new ImageCellDemoPage();
#endif
    }
}