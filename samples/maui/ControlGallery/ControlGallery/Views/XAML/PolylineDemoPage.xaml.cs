using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class PolylineDemoPage : ContentPage
    {
        public PolylineDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static PolylineDemoPage Example() => new PolylineDemoPage();
#endif
    }
}
