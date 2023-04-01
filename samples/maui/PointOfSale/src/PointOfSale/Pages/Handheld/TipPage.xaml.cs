namespace PointOfSale.Pages.Handheld;

public partial class TipPage : ContentPage
{
	public TipPage(TipViewModel vm = null)
	{
		InitializeComponent();

		if (vm != null)
			BindingContext = vm;
	}

#if EXAMPLES
    [UIExample]
    public static TipPage Default() => new(new TipViewModel());
#endif
}
