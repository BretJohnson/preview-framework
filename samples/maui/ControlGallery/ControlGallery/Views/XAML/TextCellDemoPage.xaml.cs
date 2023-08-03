using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class TextCellDemoPage : ContentPage
    {
        public TextCellDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static TextCellDemoPage Example() => new TextCellDemoPage();
#endif
    }
}