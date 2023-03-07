using System;
using System.Reflection;

namespace ExampleBook.Tooling;

public class Example
{
    private string? _description;
    private MethodInfo _methodInfo;

    public Example(string? description, MethodInfo methodInfo)
    {
        _description = description;
        _methodInfo = methodInfo;
    }

    public Example(ExampleAttribute uiExampleAttribute, MethodInfo methodInfo)
    {
        _description = uiExampleAttribute.Title;
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
    public string GetMethodDisplayName()
    {
        return $"{_methodInfo.DeclaringType.Name}.{_methodInfo.Name}";
    }

    public string? Description => _description;

    public MethodInfo MethodInfo => _methodInfo;
}
