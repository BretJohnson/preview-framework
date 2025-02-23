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
    [Preview]
    public static LoginView Preview() => new();
#endif
}