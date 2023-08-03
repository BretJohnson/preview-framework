using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    public partial class RadioButtonDemoPage : ContentPage
    {
        public RadioButtonDemoPage()
        {
            InitializeComponent();
        }

#if EXAMPLES
        [UIExample("Example")]
        public static RadioButtonDemoPage Example() => new RadioButtonDemoPage();
#endif
        void OnColorsRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            colorLabel.Text = $"You have chosen: {button.Content}";
        }

        void OnFruitsRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            fruitLabel.Text = $"You have chosen: {button.Content}";
        }
    }
}
