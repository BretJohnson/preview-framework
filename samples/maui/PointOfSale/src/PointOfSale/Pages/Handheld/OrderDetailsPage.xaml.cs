using ExampleBook;

namespace PointOfSale.Pages.Handheld;

public partial class OrderDetailsPage : ContentPage
{
	public OrderDetailsPage(OrderDetailsViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
	}

    public OrderDetailsPage() : this(new OrderDetailsViewModel())
    {
    }

#if EXAMPLES
    [UIExample]
    public static OrderDetailsPage EmptyOrder() =>
        new(
            new OrderDetailsViewModel(
                AppData.GenerateSampleOrder(7, "Ready to Order")));

    [UIExample]
    public static OrderDetailsPage SmallOrder() =>
        new(
            new OrderDetailsViewModel(
                AppData.GenerateSampleOrder(7, "Ready to Pay",
                    new Item() { Title = "Japchae", Price = 13.99, Quantity = 1, Image = "japchae.png", Category = ItemCategory.Noodles }
                )));

    [UIExample]
    public static OrderDetailsPage BigOrder() =>
        new(
            new OrderDetailsViewModel(
                AppData.GenerateSampleOrder(7, "Ready to Pay",
                    new Item() { Title = "Japchae", Price = 13.99, Quantity = 1, Image = "japchae.png", Category = ItemCategory.Noodles },
                    new Item() { Title = "Jajangmyeon", Price = 14.99, Quantity = 1, Image = "jajangmyeon.png", Category = ItemCategory.Noodles },
                    new Item() { Title = "Janchi Guksu", Price = 12.99, Quantity = 1, Image = "janchi_guksu.png", Category = ItemCategory.Noodles },
                    new Item() { Title = "Budae Jjigae", Price = 14.99, Quantity = 1, Image = "budae_jjigae.png", Category = ItemCategory.Noodles },
                    new Item() { Title = "Naengmyeon", Price = 12.99, Quantity = 1, Image = "naengmyeon.png", Category = ItemCategory.Noodles },
                    new Item() { Title = "Soda", Price = 2.50, Quantity = 1, Category = ItemCategory.Beverages, Image = "soda.png" },
                    new Item() { Title = "Iced Tea", Price = 3.50, Quantity = 1, Category = ItemCategory.Beverages, Image = "iced_tea.png" },
                    new Item() { Title = "Hot Tea", Price = 4.00, Quantity = 1, Category = ItemCategory.Beverages, Image = "tea.png" },
                    new Item() { Title = "Coffee", Price = 4.00, Quantity = 1, Category = ItemCategory.Beverages, Image = "coffee.png" },
                    new Item() { Title = "Milk", Price = 5.00, Quantity = 1, Category = ItemCategory.Beverages, Image = "milk.png" }
                )));
#endif
}
