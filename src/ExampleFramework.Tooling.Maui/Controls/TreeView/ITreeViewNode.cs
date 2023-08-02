namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public interface ITreeViewNode
{
    string Name { get; set; }
    object? Value { get; set; }
    bool IsExtended { get; set; }
}
