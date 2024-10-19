using System.Collections.ObjectModel;
using EcommerceMAUI.Model;
using EcommerceMAUI.ViewModel;

namespace EcommerceMAUI.Views;

public partial class CartView : ContentPage
{
    public CartView(ObservableCollection<ProductListModel>? products = null)
    {
        InitializeComponent();
        BindingContext = new CartViewModel(products);
    }

#if EXAMPLES
    [UIExample("Single Item Cart")]
    public static CartView BluetoothSpeaker() => new(ExampleData.GetBluetoothSpeakerProducts());

    [UIExample("Large Cart")]
    public static CartView LargeCart() => new(ExampleData.GetExampleProducts(8));
#endif
}