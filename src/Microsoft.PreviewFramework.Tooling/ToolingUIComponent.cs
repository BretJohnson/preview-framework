namespace Microsoft.PreviewFramework.Tooling;

public class ToolingUIComponent : UIComponent<ToolingUIExample>
{
    private readonly string typeName;

    internal ToolingUIComponent(string typeName, string? displayName = null) : base(displayName)
    {
        this.typeName = typeName;
    }

    public override string Name => this.typeName;
}
