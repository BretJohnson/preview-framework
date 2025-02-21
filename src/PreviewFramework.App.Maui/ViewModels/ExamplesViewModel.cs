using System.ComponentModel;

namespace ExampleFramework.App.Maui.ViewModels;

public class ExamplesViewModel : INotifyPropertyChanged
{
    public static ExamplesViewModel Instance => _lazyInstance.Value;

    public static readonly UIComponentCategory UncategorizedCategory = new UIComponentCategory("Uncategorized");

    private static readonly Lazy<ExamplesViewModel> _lazyInstance = new Lazy<ExamplesViewModel>(() => new ExamplesViewModel());

    public IExampleNavigatorService ExampleNavigatorService { get; }

    private List<UIComponentCategory> categories;
    private List<UIComponentCategoryViewModel> uiComponentCategoryViewModels;

    public List<UIComponentCategoryViewModel> ExamplesData => this.uiComponentCategoryViewModels;

    private ExamplesViewModel()
    {
        this.ExampleNavigatorService = new MauiExampleNavigatorService();

        this.categories = new List<UIComponentCategory>();
        Dictionary<UIComponentCategory, List<AppUIComponent>> uiComponentsByCategory = [];

        AppUIComponents uiComponents = ExamplesManager.Instance.UIComponents;

        // Create a list of UIComponents for each category, including an "Uncategorized" category.
        // Also save off the list of categories that are used, for sorting.
        foreach (AppUIComponent uiComponent in uiComponents.Components)
        {
            UIComponentCategory? category = uiComponent.Category;

            if (category == null)
            {
                category = UncategorizedCategory;
            }

            if (!uiComponentsByCategory.TryGetValue(category, out List<AppUIComponent>? uiComponentsForCategory))
            {
                this.categories.Add(category);
                uiComponentsForCategory = [];
                uiComponentsByCategory.Add(category, uiComponentsForCategory);
            }

            uiComponentsForCategory.Add(uiComponent);
        }

        // Sort the categories and components
        this.categories.Sort((category1, category2) => string.Compare(category1.Name, category2.Name, StringComparison.CurrentCultureIgnoreCase));
        foreach (List<AppUIComponent> componentsForCategory in uiComponentsByCategory.Values)
        {
            componentsForCategory.Sort((component1, component2) => string.Compare(component1.DisplayName, component2.DisplayName, StringComparison.CurrentCultureIgnoreCase));
        }

        this.uiComponentCategoryViewModels = new List<UIComponentCategoryViewModel>();
        foreach (UIComponentCategory category in this.categories)
        {
            this.uiComponentCategoryViewModels.Add(new UIComponentCategoryViewModel(category, uiComponentsByCategory[category]));
        }

#if false
        Examples = new ObservableCollection<Example>
        {
            new Example { Name = "Example 1", Description = "This is the first example." },
            new Example { Name = "Example 2", Description = "This is the second example." }
        };
#endif
    }

    public event PropertyChangedEventHandler? PropertyChanged = null;
}
