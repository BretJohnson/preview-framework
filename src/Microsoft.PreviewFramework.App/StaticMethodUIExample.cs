using System.Reflection;

namespace ExampleFramework.App;

public class StaticMethodUIExample : AppUIExample
{
    public MethodInfo MethodInfo { get; }

    public StaticMethodUIExample(UIExampleAttribute uiExampleAttribute, MethodInfo methodInfo) : base(uiExampleAttribute)
    {
        this.MethodInfo = methodInfo;
    }

    public override object Create()
    {
        if (this.MethodInfo.GetParameters().Length != 0)
            throw new InvalidOperationException($"Examples that take parameters aren't yet supported: {this.Name}");

        return this.MethodInfo.Invoke(null, null);
    }

    public override Type? DefaultUIComponentType => this.MethodInfo.ReturnType;

    /// <summary>
    /// FullName is intended to be what's used by the code to identify the example. It's the example's
    /// full qualified method name.
    /// </summary>
    public override string Name => this.MethodInfo.DeclaringType.FullName + "." + this.MethodInfo.Name;
}
