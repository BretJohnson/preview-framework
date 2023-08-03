using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static TabbedPageDemoPage Example() => new TabbedPageDemoPage();
#endif
    }
}