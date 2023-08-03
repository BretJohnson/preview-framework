namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public interface ITreeViewNode
{
    string Name { get; set; }
    object? Value { get; set; }
    bool IsExpanded { get; set; }
    bool IsSelected { get; set; }
}
