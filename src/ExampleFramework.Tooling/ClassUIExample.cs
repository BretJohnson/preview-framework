namespace ExampleFramework.Tooling;

public class ClassUIExample : UIExample
{
    public Type Type { get; }

    public ClassUIExample(UIExampleAttribute uiExampleAttribute, Type type) : base(uiExampleAttribute)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override object Create()
    {
        return Activator.CreateInstance(Type);
    }

    /// <inheritdoc/>
    public override string DefaultTitle => Type.Name;

    public override Type? DefaultUIComponentType => null;

    /// <inheritdoc/>
    public override string FullName => Type.FullName;
}
