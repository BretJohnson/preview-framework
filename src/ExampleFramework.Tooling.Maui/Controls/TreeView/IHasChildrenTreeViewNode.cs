using System.Collections.Specialized;

namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public interface IHasChildrenTreeViewNode : ITreeViewNode
{
    IList<IHasChildrenTreeViewNode> Children { get; }
    bool IsLeaf { get; set; }
}
