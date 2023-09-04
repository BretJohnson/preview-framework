using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public class TreeViewNode : BindableObject, ILazyLoadTreeViewNode
{
    private bool? _isLeaf;
    private string _name = string.Empty;
    private string _toolTip = string.Empty;
    private bool _isExtended;
    private bool _isSelected;
    private object? _value;

    public TreeViewNode(string name, object? value = null, bool isExtended = false, IList<IHasChildrenTreeViewNode>? children = null)
    {
        Name = name;
        Value = value;
        IsExpanded = isExtended;

        if (children != null)
        {
            Children = children;
        }
    }

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string ToolTip { get => _toolTip; set => SetProperty(ref _toolTip, value); }
    public bool IsExpanded { get => _isExtended; set => SetProperty(ref _isExtended, value); }
    public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }
    public object? Value { get => _value; set => SetProperty(ref _value, value); }
    public IList<IHasChildrenTreeViewNode> Children { get; set; } = new ObservableCollection<IHasChildrenTreeViewNode>();
    public Func<ITreeViewNode, IEnumerable<IHasChildrenTreeViewNode>>? GetChildren { get; set; }
    public bool IsLeaf { get => _isLeaf ?? !Children.Any() && GetChildren == null; set => SetProperty(ref _isLeaf, value); }

    protected virtual void SetProperty<T>(ref T field, T value, Action<T>? doAfter = null, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        OnPropertyChanged(propertyName);
        doAfter?.Invoke(value);
    }
}
