using ExampleFramework.App.Maui.ViewModels;

namespace ExampleFramework.App.Maui.Pages;

public partial class ExamplesPage : ContentPage
{
	public ExamplesPage()
	{
		this.InitializeComponent();
        this.BindingContext = ExamplesViewModel.Instance;
    }
}
