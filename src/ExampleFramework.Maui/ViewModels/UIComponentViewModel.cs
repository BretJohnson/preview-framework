using ExampleFramework.Tooling;

namespace ExampleFramework.Maui.ViewModels
{
    public class UIComponentViewModel
    {
        public string Title => this.UIComponent.Title;

        public UIComponent UIComponent { get; }

        public UIComponentViewModel(UIComponent uiComponent)
        {
            this.UIComponent = uiComponent;
        }
    }
}
