using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class RefreshViewDemoPage : ContentPage
    {
        public RefreshViewDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static RefreshViewDemoPage Example() => new RefreshViewDemoPage();
#endif
    }
}
