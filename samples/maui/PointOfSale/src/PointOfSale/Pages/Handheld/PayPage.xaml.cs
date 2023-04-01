using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace PointOfSale.Pages.Handheld;

public partial class PayPage : ContentPage
{
    public PayPage()
	{
		InitializeComponent();
	}

#if EXAMPLES
    [UIExample]
    public static PayPage Default() => new();
#endif
}
