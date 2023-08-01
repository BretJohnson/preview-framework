using TreeView.Maui.Core;

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
        IReadOnlyList<Window> appWindows = Application.Current.Windows;
        if (appWindows.Count > 0)
            appWindows[0].Title = "PointOfSale App Gallery";
    }

    private void OnExampleTapped(object sender, TappedEventArgs e)
    {
        if (sender is HorizontalStackLayout horizonalStackLayout)
        {
            var binding = horizonalStackLayout.Parent.BindingContext;
            if (binding is TreeViewNode treeViewNode)
            {
                var value = treeViewNode.Value;
                if (value is UIExample uiExample)
                {
                    object? exampleUI = uiExample.MethodInfo.Invoke(null, Array.Empty<object>());

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
