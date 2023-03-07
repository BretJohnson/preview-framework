using ExampleBook;
using WeatherTwentyOne.ViewModels;

namespace WeatherTwentyOne.Views;

public partial class CurrentWidget
{
    public CurrentWidget()
    {
        InitializeComponent();
    }

#if DEBUG
    [Example("My Example")]
    public static CurrentWidget Example() => new CurrentWidget(new HomeViewModel());
#endif
}
