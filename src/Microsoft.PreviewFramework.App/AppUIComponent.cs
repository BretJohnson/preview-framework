namespace ExampleFramework.App;

public class AppUIComponent : UIComponent<AppUIExample>
{
    private readonly Type type;

    internal AppUIComponent(Type type, string? displayName) : base(displayName)
    {
        this.type = type;
    }

    public override string Name => this.type.FullName;

    public Type Type => this.type;
}
