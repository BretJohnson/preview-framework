namespace ExampleBook.Tooling.Maui.Views;

public partial class RemoteControlMainPage : ContentPage
{
	public RemoteControlMainPage()
	{
		InitializeComponent();
    }

    public void SetExample(UIExample example)
    {
        object exampleUI = example.MethodInfo.Invoke(null, new object[0]);

        if (exampleUI is ContentPage contentPage)
        {
            ExampleWrapper.BackgroundColor = contentPage.BackgroundColor;
            ExampleWrapper.Content = contentPage.Content;
            ExampleWrapper.BindingContext = contentPage.BindingContext;
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
