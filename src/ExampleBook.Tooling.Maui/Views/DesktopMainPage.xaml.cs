using TreeView.Maui.Core;

namespace ExampleBook.Tooling.Maui.Views;

public partial class DesktopMainPage : ContentPage
{
	public DesktopMainPage()
	{
		InitializeComponent();

        this.BindingContext = new ExampleTreeViewModel();
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
                        ExampleWrapper.Content = contentPage.Content;
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
