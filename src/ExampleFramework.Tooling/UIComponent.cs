using System.Reflection;

namespace ExampleFramework.Tooling;

public class UIComponent
{
    private string? _title;
    private readonly Type _type;
    private readonly List<UIExample> _examples = new();
    private List<UIProperty>? _properties = null;

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
    /// FullName is intended to be what's used by the code to identify the component. It's just the component's
    /// full qualitified type name. It's unique.
    /// </summary>
    public string FullName => _type.FullName;

    public Type Type => _type;

    public void AddExample(UIExample example)
    {
        _examples.Add(example);
    }

    public IEnumerable<UIExample> Examples => _examples;

    public UIExample? GetExample(string name)
    {
        foreach (UIExample example in _examples)
        {
            if (example.FullName == name)
            {
                return example;
            }
        }

        return null;
    }

    public List<UIProperty> GetProperties()
    {
        if (_properties != null)
        {
            return _properties;
        }

        var properties = new List<UIProperty>();
        var typesProcessed = new HashSet<Type>();
        AddReflectionProperties(_type, typesProcessed, properties);

        _properties = properties;
        return _properties;
    }

    public static void AddReflectionProperties(Type type, HashSet<Type> typesProcessed, List<UIProperty> properties)
    {
        if (typesProcessed.Contains(type))
            return;

        foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            // Only include properties that can both be read & written
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite)
                continue;

            properties.Add(new ReflectionUIProperty(propertyInfo));
        }

        typesProcessed.Add(type);

        if (type.IsInterface)
        {
            foreach (var baseInterface in type.GetInterfaces())
            {
                AddReflectionProperties(baseInterface, typesProcessed, properties);
            }
        }
        else if (type.IsClass)
        {
            Type? baseType = type.BaseType;
            if (baseType != null)
            {
                AddReflectionProperties(baseType, typesProcessed, properties);
            }
        }
    }

    public UIExample? GetDefaultExample() => _examples.FirstOrDefault();
}
