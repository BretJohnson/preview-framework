namespace WeatherTwentyOne.Pages;

public partial class MapPage : ContentPage
{
    public MapPage()
    {
        InitializeComponent();
    }

#if EXAMPLES
    [Preview("Seattle")]
    public static MapPage Seattle() => new();

    [Preview("Raleigh")]
    public static MapPage Raleigh() => new();
#endif
}
