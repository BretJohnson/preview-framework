using TreeView.Maui.Core;

namespace ExampleFramework.Tooling.Maui.Views;

public partial class DesktopMainPage : ContentPage
{
	public DesktopMainPage()
	{
		InitializeComponent();

        this.BindingContext = new ExampleTreeViewModel();
        this.Loaded += DesktopMainPage_Loaded;
    }

    private void DesktopMainPage_Loaded(object? sender, EventArgs e)
    {
        IReadOnlyList<Window> appWindows = Application.Current.Windows;
        if (appWindows.Count > 0)
            appWindows[0].Title = "PointOfSale App Gallery";
    }

    private void OnExampleTapped(object sender, TappedEventArgs e)
    {
        if (sender is HorizontalStackLayout)
        {
            var binding = ((HorizontalStackLayout)sender).Parent.BindingContext;
            if (binding is TreeViewNode)
            {
                var value = ((TreeViewNode)binding).Value;
                if (value is UIExample uiExample)
                {
                    object exampleUI = uiExample.MethodInfo.Invoke(null, new object[0]);

                    if (exampleUI is ContentPage contentPage)
                    {
                        //ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
                        ExampleWrapper.Content = contentPage.Content;
                        ExampleWrapper.BindingContext = contentPage.BindingContext;
                        
                        ((ExampleTreeViewModel)this.BindingContext).UpdatePropertiesForObject(contentPage);
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
