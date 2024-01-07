namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public interface ILazyLoadTreeViewNode : IHasChildrenTreeViewNode
{
    Func<ITreeViewNode, IEnumerable<IHasChildrenTreeViewNode>>? GetChildren { get; }
}
