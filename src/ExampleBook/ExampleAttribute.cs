using System;

namespace ExampleBook;

/// <summary>
/// An attribute that specifies this is an example, for a control or other UI.
/// Examples can be shown in a gallery viewer, doc, etc.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ExampleAttribute : Attribute
{
    /// <summary>
    /// Optional title for the example, determining how it appears in navigation UI.
    /// "/" delimeters can be used to indicate hierarchy.
    /// </summary>
    public string? Title { get; }

    public ExampleAttribute()
    {
    }

    public ExampleAttribute(string title)
    {
        Title = title;
    }
}
