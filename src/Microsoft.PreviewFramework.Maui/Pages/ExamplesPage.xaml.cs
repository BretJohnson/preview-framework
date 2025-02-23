using Microsoft.PreviewFramework.Maui.ViewModels;

namespace Microsoft.PreviewFramework.Maui.Pages;

public partial class ExamplesPage : ContentPage
{
	public ExamplesPage()
	{
		this.InitializeComponent();
        this.BindingContext = ExamplesViewModel.Instance;
    }
}
