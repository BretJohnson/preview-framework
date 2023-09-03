namespace ExampleFramework;

/// <summary>
/// An attribute that specifies metadata for UI component that has examples.
/// It can be used explicitly specify a Title, overriding the default title
/// of the type name.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class UIExamplesAttribute : Attribute
{
    /// <summary>
    /// Optional title for the example, determining how it appears in navigation UI.
    /// "/" delimiters can be used to indicate hierarchy.
    /// </summary>
    public string? Title { get; }

    public UIExamplesAttribute()
    {
    }

    public UIExamplesAttribute(string title)
    {
        this.Title = title;
    }
}
