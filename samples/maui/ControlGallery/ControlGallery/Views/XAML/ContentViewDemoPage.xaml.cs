using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ContentViewDemoPage : ContentPage
    {
        public ContentViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ContentViewDemoPage Example() => new ContentViewDemoPage();
#endif
    }
}