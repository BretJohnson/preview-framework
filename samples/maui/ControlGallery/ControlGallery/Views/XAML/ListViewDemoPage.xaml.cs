using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ListViewDemoPage : ContentPage
    {
        public ListViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ListViewDemoPage Example() => new ListViewDemoPage();
#endif
    }
}