using System;
using System.Collections.Generic;
using System.Reflection;

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

    public string? Title
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

    public Type Type => _type;

    public void AddExample(UIExample example)
    {
        _examples.Add(example);
    }

    public IEnumerable<UIExample> Examples => _examples;
}
