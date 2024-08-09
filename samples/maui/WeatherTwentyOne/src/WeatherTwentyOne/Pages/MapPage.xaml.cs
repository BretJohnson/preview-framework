namespace WeatherTwentyOne.Pages;

public partial class MapPage : ContentPage
{
    public MapPage()
    {
        InitializeComponent();
    }

#if EXAMPLES
    [UIExample("Seattle")]
    public static MapPage Seattle() => new();

    [UIExample("Raleigh")]
    public static MapPage Raleigh() => new();
#endif
}
