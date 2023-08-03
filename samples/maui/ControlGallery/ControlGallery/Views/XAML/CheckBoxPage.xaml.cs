using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class CheckBoxPage : ContentPage
    {
        public CheckBoxPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static CheckBoxPage Example() => new CheckBoxPage();
#endif
    }
}
