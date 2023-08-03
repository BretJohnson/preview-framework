using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class GraphicsViewDemoPage : ContentPage
    {
        public GraphicsViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static GraphicsViewDemoPage Example() => new GraphicsViewDemoPage();
#endif
    }
}
