namespace PointOfSale.Pages.Handheld;

public partial class SignaturePage : ContentPage
{
	public SignaturePage(SignatureViewModel vm = null)
	{
		InitializeComponent();

		if (vm != null)
			BindingContext = vm;
	}

#if EXAMPLES
    [UIExample]
    public static SignaturePage Default() => new(new SignatureViewModel());
#endif
}
