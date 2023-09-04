using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls.Shapes;

[assembly:UIComponentCategory("Layouts",
    typeof(AbsoluteLayout),
    typeof(Grid),
    typeof(FlexLayout),
    typeof(Microsoft.Maui.Controls.Compatibility.RelativeLayout),
    typeof(StackLayout),
    typeof(HorizontalStackLayout),
    typeof(VerticalStackLayout))]

[assembly: UIComponentCategory("Collections",
    typeof(CarouselView),
    typeof(CollectionView),
    typeof(IndicatorView),
    typeof(ListView),
    typeof(Picker),
    typeof(TableView),
    typeof(TextCell),
    typeof(EntryCell),
    typeof(RadioButton),
    typeof(SwitchCell),
    typeof(ImageCell))]

[assembly: UIComponentCategory("Input",
    typeof(Button),
    typeof(CheckBox),
    typeof(DatePicker),
    typeof(Editor),
    typeof(Entry),
    typeof(TimePicker),
    typeof(ImageButton),
    typeof(SearchBar),
    typeof(Slider),
    typeof(Stepper),
    typeof(Switch))]

[assembly: UIComponentCategory("Present data",
    typeof(BlazorWebView),
    typeof(BoxView),
    typeof(Border),
    typeof(ContentView),
    typeof(Frame),
    typeof(GraphicsView),
    typeof(Image),
    typeof(Label),
    typeof(ScrollView),
    typeof(SwipeView),
    typeof(WebView),
    typeof(RefreshView))]

[assembly: UIComponentCategory("Indicate activity",
    typeof(ActivityIndicator),
    typeof(ProgressBar))]

[assembly:UIComponentCategory("Shapes",
    typeof(Ellipse),
    typeof(Line),
    typeof(Microsoft.Maui.Controls.Shapes.Path),
    typeof(Rectangle),
    typeof(Polygon),
    typeof(Polyline))]

[assembly: UIComponentCategory("Pages",
    typeof(ContentPage),
    typeof(FlyoutPage),
    typeof(NavigationPage),
    typeof(TabbedPage))]

