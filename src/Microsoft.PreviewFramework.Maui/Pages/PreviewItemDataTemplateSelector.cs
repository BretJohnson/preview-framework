using Microsoft.PreviewFramework.Maui.ViewModels;

namespace Microsoft.PreviewFramework.Maui.Pages;

public class PreviewItemDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? UIComponentTemplate { get; set; }
    public DataTemplate? PreviewTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container) =>
        (item is UIComponentViewModel) ? this.UIComponentTemplate! : this.PreviewTemplate!;
}
