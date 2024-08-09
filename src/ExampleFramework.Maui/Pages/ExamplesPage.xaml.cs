using ExampleFramework.Maui.ViewModels;

namespace ExampleFramework.Maui.Pages;

public partial class ExamplesPage : ContentPage
{
	public ExamplesPage()
	{
		this.InitializeComponent();
        this.BindingContext = ExamplesViewModel.Instance;
    }
}
