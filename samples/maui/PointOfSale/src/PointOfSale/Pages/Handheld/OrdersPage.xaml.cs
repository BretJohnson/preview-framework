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

#if EXAMPLES
    [UIExample]
    public static OrdersPage Default() => new();
#endif
}
