using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class EntryDemoPage : ContentPage
    {
        public EntryDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static EntryDemoPage Example() => new EntryDemoPage();
#endif
    }
}