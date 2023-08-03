using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ImageButtonDemoPage : ContentPage
    {
        int clickTotal;

        public ImageButtonDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ImageButtonDemoPage Example() => new ImageButtonDemoPage();
#endif

        void OnImageButtonClicked(object sender, EventArgs e)
        {
            clickTotal += 1;
            label.Text = $"{clickTotal} ImageButton click{(clickTotal == 1 ? "" : "s")}";
        }
    }
}
