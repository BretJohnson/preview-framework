using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class TableViewFormDemoPage : ContentPage
    {
        public TableViewFormDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static TableViewFormDemoPage Example() => new TableViewFormDemoPage();
#endif
    }
}