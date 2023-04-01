namespace PointOfSale.Pages.Handheld;

public partial class ReceiptPage : ContentPage
{
	public ReceiptPage(ReceiptViewModel vm = null)
	{
		InitializeComponent();

		if (vm != null)
			BindingContext = vm;
	}

#if EXAMPLES
    [UIExample]
    public static ReceiptPage Default() => new(new ReceiptViewModel());
#endif
}
