using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class PolygonDemoPage : ContentPage
    {
        public PolygonDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static PolygonDemoPage Example() => new PolygonDemoPage();
#endif
    }
}
