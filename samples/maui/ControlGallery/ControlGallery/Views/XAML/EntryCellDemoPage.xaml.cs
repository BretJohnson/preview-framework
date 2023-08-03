using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class EntryCellDemoPage : ContentPage
    {
        public EntryCellDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static EntryCellDemoPage Example() => new EntryCellDemoPage();
#endif
    }
}