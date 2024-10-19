using EcommerceMAUI.ViewModel;

namespace EcommerceMAUI.Views;

public partial class HomePageView : ContentPage
{
    public HomePageView()
    {
        InitializeComponent();
        BindingContext = new HomePageViewModel();
    }

#if EXAMPLES
    [UIExample]
    public static LoginView Example() => new();
#endif
}