using System.Reflection;

namespace ExampleFramework.Tooling;

public class UIExample
{
    private readonly string? _title;

    public MethodInfo MethodInfo { get; }

    public UIExample(string? title, MethodInfo methodInfo)
    {
        _title = title;
        this.MethodInfo = methodInfo;
    }

    public UIExample(UIExampleAttribute uiExampleAttribute, MethodInfo methodInfo)
    {
        _title = uiExampleAttribute.Title;
        this.MethodInfo = methodInfo;
    }

    public object Create()
    {
        if (this.MethodInfo.GetParameters().Length != 0)
            throw new InvalidOperationException($"Examples that take parameters aren't yet supported: {GetMethodDisplayName()}");

        return this.MethodInfo.Invoke(null, null);
    }

    /// <summary>
    /// Get a user friendly display title for the example method, suitable for error
    /// messages.
    /// </summary>
    /// <returns>user friendly title of example method</returns>
    public string GetMethodDisplayName() =>
        $"{this.MethodInfo.DeclaringType.Name}.{this.MethodInfo.Name}";

    /// <summary>
    /// Title is intended to be what's shown in the UI to identify the example. It can contain spaces and
    /// isn't necessarily unique. It defaults to the method name but can be overridden by the developer.
    /// </summary>
    public string Title
    {
        get
        {
            // If there's a title explicitly set, use it
            if (_title != null)
                return _title;

            // Otherwise default to the method name
            return this.MethodInfo.Name;
        }
    }

    /// <summary>
    /// FullName is intended to be what's used by the code to identify the example. It's the examples's
    /// full qualitified method name.
    /// </summary>
    public string FullName => this.MethodInfo.DeclaringType.FullName + "." + this.MethodInfo.Name;
}
