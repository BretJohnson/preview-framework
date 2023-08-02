using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using ExampleFramework.Tooling.Maui.Controls.TreeView;

namespace ExampleFramework.Tooling.Maui;


//For Each Public Property in Bound View Models Type
//Is it a primitive
//Display it and its type
//Else
//For Each Public Property in Property
// Recurse


//Add To List
//Either as Header
//Or Entry

public class GalleryViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public List<PropertyDefinition> SelectedItemsProperties { get; set; } = new();

    public GalleryViewModel()
    {
        this.InitializeTreeView(AppUIExamplesManager.Instance.UIComponents.Components);
        this.SelectedItemsProperties = new List<PropertyDefinition>();
    }

    public ObservableCollection<TreeViewNode> Nodes { get; set; } = new();

    // For now ignore the case where the example Titles contain "/" characters for a deeper
    // hierarchy
    public void InitializeTreeView(IEnumerable<UIComponent> components)
    {
        foreach (UIComponent component in components)
        {
            var componentNode = new TreeViewNode(component.Title, component);
            foreach (UIExample example in component.Examples)
            {
                var exampleNode = new TreeViewNode(example.Title, example);
                componentNode.Children.Add(exampleNode);
            }

            this.Nodes.Add(componentNode);
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItemsProperties"));
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
