using ExampleBook;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace PointOfSale.Pages.Handheld;

public partial class PayPage : ContentPage
{
    public PayPage()
	{
		InitializeComponent();
	}

#if DEBUG
    [UIExample]
    public static PayPage Example() => new();
#endif
}
