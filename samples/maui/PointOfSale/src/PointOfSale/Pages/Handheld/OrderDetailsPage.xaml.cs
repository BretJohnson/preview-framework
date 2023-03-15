using ExampleBook;

namespace PointOfSale.Pages.Handheld;

public partial class OrderDetailsPage : ContentPage
{
	public OrderDetailsPage()
	{
		InitializeComponent();
	}

#if DEBUG
	[UIExample]
	public static OrderDetailsPage Default() => new();
#endif
}
