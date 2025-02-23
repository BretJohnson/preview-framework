using Microsoft.PreviewFramework.App;

namespace Microsoft.PreviewFramework.Maui.ViewModels;

public class UIComponentCategoryViewModel : List<object>
{
    public string Name { get; }

    public UIComponentCategoryViewModel(UIComponentCategory category, List<AppUIComponent> uiComponents)
    {
        this.Name = category.Name;
        foreach (AppUIComponent uiComponent in uiComponents)
        {
            this.Add(new UIComponentViewModel(uiComponent));

            foreach (AppPreview preview in uiComponent.Previews)
            {
                this.Add(new PreviewViewModel(preview));
            }
        }
    }
}
