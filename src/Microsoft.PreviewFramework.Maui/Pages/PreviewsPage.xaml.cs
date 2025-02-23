using Microsoft.PreviewFramework.Maui.ViewModels;

namespace Microsoft.PreviewFramework.Maui.Pages;

public partial class PreviewsPage : ContentPage
{
	public PreviewsPage()
	{
		this.InitializeComponent();
        this.BindingContext = PreviewsViewModel.Instance;
    }
}
