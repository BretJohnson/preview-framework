using EcommerceMAUI.Model;
using EcommerceMAUI.ViewModel;
using System.Collections.ObjectModel;

namespace EcommerceMAUI.Views;

public partial class CardView : ContentPage
{
    public CardView(ObservableCollection<CardInfoModel>? cards = null)
	{
		InitializeComponent();
		BindingContext = new CardViewModel(cards);
    }

#if EXAMPLES
    [UIExample("No Cards")]
    public static CardView NoCards() => new(ExampleData.GetExampleCards(0));

    [UIExample("Single Card")]
    public static CardView SingleCard() => new(ExampleData.GetExampleCards(1));

    [UIExample("Two Cards")]
    public static CardView TwoCards() => new(ExampleData.GetExampleCards(2));

    [UIExample("Six Cards")]
    public static CardView SixCards() => new(ExampleData.GetExampleCards(6));

#endif
}