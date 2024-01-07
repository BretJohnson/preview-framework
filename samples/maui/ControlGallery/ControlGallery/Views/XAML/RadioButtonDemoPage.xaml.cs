using Microsoft.Maui.Controls;

namespace ControlGallery.Views.XAML
{
    [UIExample(typeof(RadioButton))]
    public partial class RadioButtonDemoPage : ContentPage
    {
        public RadioButtonDemoPage()
        {
            InitializeComponent();
        }

        void OnColorsRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            colorLabel.Text = $"You have chosen: {button.Content}";
        }

        void OnFruitsRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            fruitLabel.Text = $"You have chosen: {button.Content}";
        }
    }
}
