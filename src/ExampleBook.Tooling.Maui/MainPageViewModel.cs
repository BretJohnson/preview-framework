using System.Collections.ObjectModel;
using System.ComponentModel;
using TreeView.Maui.Core;

namespace ExampleBook.Tooling.Maui;

public class ExampleTreeViewModel : BindableObject, INotifyPropertyChanged
{
    private String node; 
    public event PropertyChangedEventHandler PropertyChanged;

    public ExampleTreeViewModel()
    {
        InitializeTreeView(CurrentAppUIExamplesManager.Instance.UIComponents.Components);
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
}
