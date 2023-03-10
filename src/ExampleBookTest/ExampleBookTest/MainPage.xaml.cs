using TreeView.Maui.Core;

namespace ExampleBookTest;

public partial class MainPage : ContentPage
{

    public TreeViewNode node { get; set; }
    public MainPageViewModel vm {get; set; }

	public MainPage()
    {

        InitializeComponent();

        vm = new MainPageViewModel();

        this.BindingContext = vm;

     

        Binding nodeBinding = new Binding();
        nodeBinding.Mode = BindingMode.Default;
        nodeBinding.Source = vm;
        nodeBinding.Path = "SelectedNodeContent";

        label1.SetBinding(Label.TextProperty, nodeBinding);
    }

    void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if (sender is HorizontalStackLayout)
        {
            var binding = ((HorizontalStackLayout)sender).Parent.BindingContext;
            if (binding is TreeViewNode)
            {
                if (((TreeViewNode)binding).IsLeaf)
                {
                    this.vm.SelectedNodeContent = ((TreeViewNode)binding).Name;
                }
            }
        }
    }
}


