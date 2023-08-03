using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class RectangleDemoPage : ContentPage
    {
        public RectangleDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static RadioButtonDemoPage Example() => new RadioButtonDemoPage();
#endif
    }
}
