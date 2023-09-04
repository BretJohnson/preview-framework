using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using ExampleFramework.Tooling.Maui.Controls.TreeView;

namespace ExampleFramework.Tooling.Maui.ViewModels;

public class GalleryViewModel : INotifyPropertyChanged
{
    public static UIComponentCategory UncatgegorizedCategory = new UIComponentCategory("Uncategorized");

    private List<UIComponentCategory> _categories;
    private Dictionary<UIComponentCategory, List<UIComponent>> _componentsByCategory;

    public ObservableCollection<TreeViewNode> Nodes { get; set; } = new();
    public event PropertyChangedEventHandler? PropertyChanged;
    public List<PropertyDefinition> SelectedItemsProperties { get; set; } = new();

    public GalleryViewModel()
    {
        _categories = new List<UIComponentCategory>();
        _componentsByCategory = new Dictionary<UIComponentCategory, List<UIComponent>>();

        UIComponents uiComponents = AppUIExamplesManager.Instance.UIComponents;

        // Create a list of UIComponents for each category, including an "Uncategorized" category.
        // Also save off the list of categories that are used, for sorting.
        foreach (UIComponent component in uiComponents.Components)
        {
            UIComponentCategory? category = component.Category;

            if (category == null)
            {
                category = UncatgegorizedCategory;
            }

            if (! _componentsByCategory.TryGetValue(category, out List<UIComponent>? componentsForCategory))
            {
                _categories.Add(category);
                componentsForCategory = new List<UIComponent>();
                _componentsByCategory.Add(category, componentsForCategory);
            }

            componentsForCategory.Add(component);
        }

        // Sort the categories and components
        _categories.Sort((category1, category2) => string.Compare(category1.Name, category2.Name, StringComparison.CurrentCultureIgnoreCase));
        foreach (List<UIComponent> componentsForCategory in _componentsByCategory.Values)
        {
            componentsForCategory.Sort((component1, component2) => string.Compare(component1.Title, component2.Title, StringComparison.CurrentCultureIgnoreCase));
        }

        InitializeTreeView();
        SelectedItemsProperties = new List<PropertyDefinition>();
    }

    // For now ignore the case where the example Titles contain "/" characters for a deeper
    // hierarchy
    private void InitializeTreeView()
    {
        bool onlyUncategorized = _categories.Count == 1 && ReferenceEquals(_categories[0], UncatgegorizedCategory);

        foreach (UIComponentCategory category in _categories)
        {
            TreeViewNode? categoryNode = null;
            if (! onlyUncategorized)
            {
                categoryNode = new TreeViewNode(category.Name, category);
                Nodes.Add(categoryNode);
            }

            foreach (UIComponent component in _componentsByCategory[category])
            {
                var componentNode = new TreeViewNode(component.Title, component);
                if (component.ExamplesCount > 1)
                {
                    foreach (UIExample example in component.Examples)
                    {
                        componentNode.Children.Add(new TreeViewNode(example.Title, example));
                    }
                }

                if (categoryNode != null)
                {
                    categoryNode.Children.Add(componentNode);
                }
                else
                {
                    Nodes.Add(componentNode);
                }
            }
        }
    }

    public void UpdatePropertiesForObject(object obj)
    {
        PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        // sort properties by name
        Array.Sort(propertyInfos,
                   (propertyInfo1, propertyInfo2) => propertyInfo1.Name.CompareTo(propertyInfo2.Name));

        var properties = new List<PropertyDefinition>();
        foreach (PropertyInfo p in propertyInfos)
        {
            string name = p.Name;
            object? prop = p.GetValue(obj);
            if (prop != null && p.GetValue(obj)?.ToString() != null)
            {
                var val = p.GetValue(obj).ToString();
                properties.Add(new PropertyDefinition(name, val));
            }
        }
        properties.Add(new PropertyDefinition("", ""));

        this.SelectedItemsProperties = properties;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedItemsProperties)));
    }

    public class PropertyDefinition
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public PropertyDefinition(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
