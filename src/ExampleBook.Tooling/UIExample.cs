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

    public UIExample(ExampleAttribute uiExampleAttribute, MethodInfo methodInfo)
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

    public string? Title => _title;

    public MethodInfo MethodInfo => _methodInfo;
}
