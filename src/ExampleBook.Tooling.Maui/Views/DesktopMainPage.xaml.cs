namespace ExampleBook.Tooling.Maui.Views;

public partial class DesktopMainPage : ContentPage
{
	public DesktopMainPage()
	{
		InitializeComponent();
	}

    private void OnExampleSelected(object sender, SelectedItemChangedEventArgs e)
    {
        UIExample? currentSelection = e.SelectedItem as UIExample;

        if (currentSelection != null)
        {
            object exampleUI = currentSelection.MethodInfo.Invoke(null, new object[0]);

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
