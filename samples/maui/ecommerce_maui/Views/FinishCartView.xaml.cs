using EcommerceMAUI.Model;
using EcommerceMAUI.ViewModel;
using System.Collections.ObjectModel;

namespace EcommerceMAUI.Views;

public partial class FinishCartView : ContentPage
{
	public FinishCartView(ObservableCollection<ProductListModel> products, DeliveryTypeModel deliveryType, AddressModel address, CardInfoModel card)
	{
		InitializeComponent();
		BindingContext = new FinishCartViewModel(products, deliveryType, address, card);
    }

#if EXAMPLES
    [UIExample()]
    public static FinishCartView Default() => new(
		new ObservableCollection<ProductListModel>(ProductListModel.GetExampleProducts()),
		new DeliveryTypeModel(), new AddressModel(), new CardInfoModel());
#endif
}