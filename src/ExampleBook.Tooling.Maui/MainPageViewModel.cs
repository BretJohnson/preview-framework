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
        /*
        List<UIExample> list = new List<UIExample>();
        list.Add(new UIExample("MainPage", null));
        list.Add(new UIExample("MainPage/Child1", null));
        list.Add(new UIExample("MainPage/Child2", null));

        list.Add(new UIExample("MainPage2", null));
        list.Add(new UIExample("MainPage2/Child1A", null));
        list.Add(new UIExample("MainPage2/Child2B", null));
        list.Add(new UIExample("MainPage2/Child2B/Child2BElement", null));
        */

        InitializeTreeView(CurrentAppUIExamplesManager.Instance.UIExamples.AllExamples);
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

    //Doesn't Allow Duplicate Names.
    public void InitializeTreeView(IEnumerable<UIExample> examples)
    {
        Dictionary<string, TreeViewNode> nodes = new Dictionary<string, TreeViewNode>();
        foreach (UIExample exp in examples)
        {
            string name;
            if (exp.Title.Contains('/'))
            {
                var splitName = exp.Title.Split('/');
                name = splitName[splitName.Length - 1];
                var node = new TreeViewNode(name, exp);
                nodes.Add(name, node);
            }
            else
            {
                name = exp.Title;
                var node = new TreeViewNode(name, exp);
                nodes.Add(name, node);

            }
        }

        foreach (TreeViewNode node in nodes.Values)
        {
            var val = (UIExample)node.Value;

            if (val.Title.Contains('/'))
            {
                var splitName = val.Title.Split('/');
                nodes[splitName[splitName.Length - 2]].Children.Add(node);
            }
            else
            {
                Nodes.Add(node);
                    
            }
        }
    }
}
