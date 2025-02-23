namespace Microsoft.PreviewFramework.Tooling;

public abstract class ToolingUIExample : UIExample
{
    private readonly string name;

    public ToolingUIExample(string name, string? displayName) : base(displayName)
    {
        this.name = name;
    }

    public override string Name => this.name;
}
