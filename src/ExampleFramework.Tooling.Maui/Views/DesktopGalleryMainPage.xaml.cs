using ExampleFramework.Tooling.Maui.Controls.TreeView;
using ExampleFramework.Tooling.Maui.ViewModels;

namespace ExampleFramework.Tooling.Maui.Views;

public partial class DesktopGalleryMainPage : ContentPage
{
	public DesktopGalleryMainPage()
	{
        this.InitializeComponent();

        this.BindingContext = new GalleryViewModel();
        this.Loaded += this.DesktopMainPage_Loaded;
    }

    private void DesktopMainPage_Loaded(object? sender, EventArgs e)
    {
        IReadOnlyList<Window> appWindows = Application.Current!.Windows;
        if (appWindows.Count > 0)
            appWindows[0].Title = "PointOfSale App Gallery";
    }

    private void OnTreeNodeTapped(object sender, TappedEventArgs e)
    {
        if (sender is HorizontalStackLayout horizonalStackLayout)
        {
            var binding = horizonalStackLayout.Parent.BindingContext;
            if (binding is TreeViewNode treeViewNode)
            {
                NavigationTree.SelectedItem = treeViewNode;

                var value = treeViewNode.Value;

                UIExample? uiExample = null;
                if (value is UIComponent uiComponent)
                {
                    uiExample = uiComponent.GetDefaultExample();
                }
                else if (value is UIExample uiExampleValue)
                {
                    uiExample = uiExampleValue;
                }

                if (uiExample != null)
                {
                    object? exampleUI = uiExample.Create();

                    if (exampleUI is ContentPage contentPage)
                    {
                        //ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
                        ExampleWrapper.Content = contentPage.Content;
                        ExampleWrapper.BindingContext = contentPage.BindingContext;
                        
                        ((GalleryViewModel)this.BindingContext).UpdatePropertiesForObject(contentPage);
                    }
                    else if (exampleUI is View view)
                    {
                        ExampleWrapper.Content = view;
                    }
                    else
                    {
                        ExampleWrapper.Content = null;
                    }
                }
            }
        }
    }
}
