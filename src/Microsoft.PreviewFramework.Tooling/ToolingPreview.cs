namespace Microsoft.PreviewFramework.Tooling;

public abstract class ToolingPreview : Preview
{
    private readonly string name;

    public ToolingPreview(string name, string? displayName) : base(displayName)
    {
        this.name = name;
    }

    public override string Name => this.name;
}
