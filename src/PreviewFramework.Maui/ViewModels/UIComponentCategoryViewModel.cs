using ExampleFramework.App.Maui;

namespace ExampleFramework.App.Maui.ViewModels;

public class UIComponentCategoryViewModel : List<object>
{
    public string Name { get; }

    public UIComponentCategoryViewModel(UIComponentCategory category, List<AppUIComponent> uiComponents)
    {
        this.Name = category.Name;
        foreach (AppUIComponent uiComponent in uiComponents)
        {
            this.Add(new UIComponentViewModel(uiComponent));

            foreach (AppUIExample example in uiComponent.Examples)
            {
                this.Add(new ExampleViewModel(example));
            }
        }
    }
}
