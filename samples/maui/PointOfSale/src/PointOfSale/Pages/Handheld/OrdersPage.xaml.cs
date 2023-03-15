using ExampleBook;

namespace PointOfSale.Pages.Handheld;

public partial class OrdersPage : ContentPage
{
    public OrdersPage(OrdersViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public OrdersPage() : this(null)
	{
    }

#if DEBUG
    [UIExample]
    public static OrdersPage Default() => new();
#endif
}
