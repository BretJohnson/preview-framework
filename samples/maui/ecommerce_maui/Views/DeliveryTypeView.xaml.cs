using EcommerceMAUI.Model;
using EcommerceMAUI.ViewModel;
using System.Collections.ObjectModel;

namespace EcommerceMAUI.Views;

public partial class DeliveryTypeView : ContentPage
{
	public DeliveryTypeView(ObservableCollection<ProductListModel> products)
	{
		InitializeComponent();
        BindingContext = new DeliveryTypeViewModel(products);
    }

#if EXAMPLES
    [UIExample()]
    public static DeliveryTypeView Default() => new(ProductListModel.GetExampleProducts());
#endif
}