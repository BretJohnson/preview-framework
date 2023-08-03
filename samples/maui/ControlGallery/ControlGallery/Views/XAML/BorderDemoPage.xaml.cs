using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class BorderDemoPage : ContentPage
    {
        public BorderDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static BorderDemoPage Example() => new BorderDemoPage();
#endif
    }
}