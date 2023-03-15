using System;
using System.Reflection;

namespace ExampleBook.Tooling;

public class UIExample
{
    private string? _title;
    private MethodInfo _methodInfo;

    public UIExample(string? title, MethodInfo methodInfo)
    {
        _title = title;
        _methodInfo = methodInfo;
    }

    public UIExample(UIExampleAttribute uiExampleAttribute, MethodInfo methodInfo)
    {
        _title = uiExampleAttribute.Title;
        _methodInfo = methodInfo;
    }

    public object Create()
    {
        if (_methodInfo.GetParameters().Length != 0)
            throw new InvalidOperationException($"Examples that take parameters aren't yet supported: {GetMethodDisplayName()}");

        return _methodInfo.Invoke(null, null);
    }

    /// <summary>
    /// Get a user friendly display name for the example method, suitable for error
    /// messages.
    /// </summary>
    /// <returns>user friendly name of example method</returns>
    public string GetMethodDisplayName() =>
        $"{_methodInfo.DeclaringType.Name}.{_methodInfo.Name}";

    public string? Title
    {
        get
        {
            // If there's a title explicitly set, use it
            if (_title != null)
                return _title;

            // Otherwise default to the component type name
            return _methodInfo.ReturnType.Name;
        }
    }

    public MethodInfo MethodInfo => _methodInfo;
}
