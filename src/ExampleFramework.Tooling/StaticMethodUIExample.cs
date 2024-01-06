using System.Reflection;

namespace ExampleFramework.Tooling;

public class StaticMethodUIExample : UIExample
{
    public MethodInfo MethodInfo { get; }

    public StaticMethodUIExample(UIExampleAttribute uiExampleAttribute, MethodInfo methodInfo) : base(uiExampleAttribute)
    {
        MethodInfo = methodInfo;
    }

    public override object Create()
    {
        if (MethodInfo.GetParameters().Length != 0)
            throw new InvalidOperationException($"Examples that take parameters aren't yet supported: {FullName}");

        return MethodInfo.Invoke(null, null);
    }

    /// <inheritdoc/>
    public override string DefaultTitle => MethodInfo.Name;

    public override Type? DefaultUIComponentType => MethodInfo.ReturnType;

    /// <summary>
    /// FullName is intended to be what's used by the code to identify the example. It's the example's
    /// full qualified method name.
    /// </summary>
    public override string FullName => MethodInfo.DeclaringType.FullName + "." + MethodInfo.Name;
}
