using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using TreeView.Maui.Core;

namespace ExampleBook.Tooling.Maui;


//For Each Public Property in Bound View Models Type
//Is it a primitive
//Display it and its type
//Else
//For Each Public Property in Property
// Recurse


//Add To List
//Either as Header
//Or Entry

public class ExampleTreeViewModel : BindableObject, INotifyPropertyChanged
{
    private String node; 
    public event PropertyChangedEventHandler PropertyChanged;

    public List<PropertyDefinition> SelectedItemsProperties { get; set; } = new();

    public ExampleTreeViewModel()
    {
        InitializeTreeView(UIExamplesManager.Instance.UIComponents.Components);
        SelectedItemsProperties = new List<PropertyDefinition>();
        //SelectedItemsProperties.Add(new PropertyDefinition("test1", "99"));
        //SelectedItemsProperties.Add(new PropertyDefinition("test2", "89"));
        InitializeTreeView(UIExamplesManager.Instance.UIComponents.Components);
    }

    public ObservableCollection<TreeViewNode> Nodes { get; set; } = new();
    public String SelectedNodeContent {
        get
        {
            return node;
        }
        set
        {
            if (node != value)
            {
                node = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNodeContent"));
            }
        }
    }

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

            Nodes.Add(componentNode);
        }
    }

    public void GetPropertiesForObject(object obj)
    {
        var propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);//By default, it will return only public properties.
        // sort properties by name
        Array.Sort(propertyInfos,
                   (propertyInfo1, propertyInfo2) => propertyInfo1.Name.CompareTo(propertyInfo2.Name));

        SelectedItemsProperties.Clear();
        foreach (PropertyInfo p in propertyInfos)
        {
            var name = p.Name;
            var prop = p.GetValue(obj);
            if (prop != null && p.GetValue(obj)?.ToString() != null)
            {
                var val = p.GetValue(obj).ToString();
                SelectedItemsProperties.Add(new PropertyDefinition(name, val));
            }
        }
        SelectedItemsProperties.Add(new PropertyDefinition("", ""));

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
