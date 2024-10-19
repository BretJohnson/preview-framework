namespace ExampleFramework.App.Maui.ViewModels;

public class UIComponentViewModel
{
    public string DisplayName => this.UIComponent.DisplayName;

    public AppUIComponent UIComponent { get; }

    public UIComponentViewModel(AppUIComponent uiComponent)
    {
        this.UIComponent = uiComponent;
    }
}
