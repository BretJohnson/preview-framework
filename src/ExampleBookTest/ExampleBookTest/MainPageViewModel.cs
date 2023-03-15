using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using TreeView.Maui.Core;

namespace ExampleBookTest
{
	public class ExampleTreeViewModel : BindableObject, INotifyPropertyChanged
    {
        private String node; 
        public event PropertyChangedEventHandler PropertyChanged;

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
        public void InitializeTreeView(List<UIExample> examples)
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

        public ExampleTreeViewModel()
        {
            List<UIExample> list = new List<UIExample>();
            list.Add(new UIExample("MainPage", null));
            list.Add(new UIExample("MainPage/Child1", null));
            list.Add(new UIExample("MainPage/Child2", null));

            list.Add(new UIExample("MainPage2", null));
            list.Add(new UIExample("MainPage2/Child1A", null));
            list.Add(new UIExample("MainPage2/Child2B", null));
            list.Add(new UIExample("MainPage2/Child2B/Child2BElement", null));

            InitializeTreeView(list);
        }
    }

    public class UIExample
    {
        private string? _title;
        private MethodInfo _methodInfo;

        public UIExample(string? title, MethodInfo methodInfo)
        {
            _title = title;
            _methodInfo = methodInfo;
        }

        public UIExample(ExampleAttribute uiExampleAttribute, MethodInfo methodInfo)
        {
            _title = uiExampleAttribute.Title;
            _methodInfo = methodInfo;
        }

        public object Create()
        {
            if (_methodInfo.GetParameters().Length != 0)
                throw new InvalidOperationException($"Examples that take parameters aren't yet supported: {GetMethodDisplayName()}");

            return _methodInfo.Invoke(null, null);
        }

        /// <summary>
        /// Get a user friendly display name for the example method, suitable for error
        /// messages.
        /// </summary>
        /// <returns>user friendly name of example method</returns>
        public string GetMethodDisplayName() =>
            $"{_methodInfo.DeclaringType.Name}.{_methodInfo.Name}";

        public string? Title => _title;

        public MethodInfo MethodInfo => _methodInfo;
    }

    /// <summary>
    /// An attribute that specifies this is an example, for a control or other UI.
    /// Examples can be shown in a gallery viewer, doc, etc.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExampleAttribute : Attribute
    {
        /// <summary>
        /// Optional title for the example, determining how it appears in navigation UI.
        /// "/" delimeters can be used to indicate hierarchy.
        /// </summary>
        public string? Title { get; }

        public ExampleAttribute()
        {
        }

        public ExampleAttribute(string title)
        {
            Title = title;
        }
    }
}

