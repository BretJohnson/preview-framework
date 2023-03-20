using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleBook.Tooling;

public class UIComponent
{
    private string? _title;
    private readonly Type _type;
    private readonly List<UIExample> _examples = new();

    public UIComponent(string? title, Type type)
    {
        _title = title;
        _type = type;
    }

    /// <summary>
    /// Title is intended to be what's shown in the UI to identify the component. It can contain spaces and
    /// isn't necessarily unique. It defaults to the class name (with no namespace qualifier) but can be
    /// overridden by the developer.
    /// </summary>
    public string Title
    {
        get
        {
            // If there's a title explicitly set, use it
            if (_title != null)
                return _title;

            // Otherwise default to the component type name
            return _type.Name;
        }
    }

    /// <summary>
    /// Name is intended to be what's used by the code to identify the component. It's just the component's
    /// full qualitified type name. It's unique.
    /// </summary>
    public string Name => _type.FullName;

    public Type Type => _type;

    public void AddExample(UIExample example)
    {
        _examples.Add(example);
    }

    public IEnumerable<UIExample> Examples => _examples;

    public UIExample? GetExampleWithName(string name)
    {
        foreach (UIExample example in _examples)
        {
            if (example.Name == name)
            {
                return example;
            }
        }

        return null;
    }

    public UIExample? GetDefaultExample() => _examples.FirstOrDefault();
}
