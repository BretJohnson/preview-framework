using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class LineDemoPage : ContentPage
    {
        public LineDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static LineDemoPage Example() => new LineDemoPage();
#endif
    }
}
