using ExampleFramework.Tooling;

namespace ExampleFramework.Maui.ViewModels;

public class UIComponentCategoryViewModel : List<object>
{
    public string Name { get; }

    public UIComponentCategoryViewModel(UIComponentCategory category, List<UIComponent> uiComponents)
    {
        this.Name = category.Name;
        foreach (UIComponent component in uiComponents)
        {
            this.Add(new UIComponentViewModel(component));

            foreach (UIExample example in component.Examples)
            {
                this.Add(new ExampleViewModel(example));
            }
        }
    }
}
