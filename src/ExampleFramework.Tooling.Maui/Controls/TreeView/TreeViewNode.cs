using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public class TreeViewNode : BindableObject, ILazyLoadTreeViewNode
{
    private bool? _isLeaf;
    private string _name = string.Empty;
    private bool _isExtended;
    private bool _isSelected;
    private object? _value;

    public TreeViewNode(string name, object? value = null, bool isExtended = false, IList<IHasChildrenTreeViewNode>? children = null)
    {
        this.Name = name;
        this.Value = value;
        this.IsExpanded = isExtended;

        if (children != null)
        {
            this.Children = children;
        }
    }

    public virtual string Name { get => _name; set => this.SetProperty(ref _name, value); }
    public virtual bool IsExpanded { get => _isExtended; set => this.SetProperty(ref _isExtended, value); }
    public virtual bool IsSelected { get => _isSelected; set => this.SetProperty(ref _isSelected, value); }
    public virtual object? Value { get => _value; set => this.SetProperty(ref _value, value); }
    public virtual IList<IHasChildrenTreeViewNode> Children { get; set; } = new ObservableCollection<IHasChildrenTreeViewNode>();
    public virtual Func<ITreeViewNode, IEnumerable<IHasChildrenTreeViewNode>>? GetChildren { get; set; }
    public virtual bool IsLeaf { get => _isLeaf ?? !this.Children.Any() && this.GetChildren == null; set => this.SetProperty(ref _isLeaf, value); }

    protected virtual void SetProperty<T>(ref T field, T value, Action<T>? doAfter = null, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
        doAfter?.Invoke(value);
    }
}
