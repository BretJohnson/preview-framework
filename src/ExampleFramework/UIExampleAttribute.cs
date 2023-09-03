namespace ExampleFramework;

/// <summary>
/// An attribute that specifies this is an example, for a control or other UI.
/// Examples can be shown in a gallery viewer, doc, etc.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class UIExampleAttribute : Attribute
{
    /// <summary>
    /// Optional title for the example, determining how it appears in navigation UI.
    /// "/" delimiters can be used to indicate hierarchy.
    /// </summary>
    public string? Title { get; }

    public Type? UIComponentType { get; }

    public UIExampleAttribute()
    {
    }

    public UIExampleAttribute(string? title = null, Type? uiComponent = null)
    {
        Title = title;
        UIComponentType = uiComponent;
    }

    public UIExampleAttribute(Type uiComponent)
    {
        UIComponentType = uiComponent;
    }
}
