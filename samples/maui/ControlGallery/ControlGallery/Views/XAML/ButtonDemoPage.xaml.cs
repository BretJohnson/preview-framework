using System;
using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class ButtonDemoPage : ContentPage
    {
        static int clickTotal;

        public ButtonDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static ButtonDemoPage Example() => new ButtonDemoPage();
#endif

        void OnButtonClicked(object sender, EventArgs e)
        {
            clickTotal += 1;
            label.Text = String.Format("{0} button click{1}",
                                       clickTotal, clickTotal == 1 ? "" : "s");
        }
    }
}